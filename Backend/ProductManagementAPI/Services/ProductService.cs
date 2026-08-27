using Azure.Storage.Files.Shares;
using Azure;
using Microsoft.Azure.Cosmos;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Interfaces;
using ProductManagementAPI.Models;
using System.Text.Json;
using System.Text;

namespace ProductManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly Container _container;
        private readonly ShareClient _shareClient;

        public ProductService(CosmosClient cosmosClient, ShareServiceClient shareServiceClient, IConfiguration configuration)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);

            var shareName = configuration["AzureFileStorage:ShareName"];
            _shareClient = shareServiceClient.GetShareClient(shareName);
        }

        public async Task<Product> CreateProductAsync(CreateProductDto dto)
        {
            // DTO ko Model mein convert kiya taake ID auto-generate ho jaye
            var newProduct = new Product
            {
                Category = dto.Category,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

            await _container.CreateItemAsync(newProduct, new PartitionKey(newProduct.Category));

            try
            {
                var directoryClient = _shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient($"{newProduct.Id}.json");
                string jsonString = JsonSerializer.Serialize(newProduct);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

                using (var stream = new MemoryStream(bytes))
                {
                    await fileClient.CreateAsync(stream.Length);
                    await fileClient.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Upload Error: {ex.Message}");
            }

            return newProduct;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var query = _container.GetItemQueryIterator<Product>(new QueryDefinition("SELECT * FROM c"));
            var results = new List<Product>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            var queryDef = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id);
            var query = _container.GetItemQueryIterator<Product>(queryDef);
            if (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                return response.FirstOrDefault();
            }
            return null;
        }

        public async Task<bool> UpdateProductAsync(string id, UpdateProductDto dto)
        {
            var existingProduct = await GetProductByIdAsync(id);
            if (existingProduct == null) return false;

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Category = dto.Category;

            await _container.ReplaceItemAsync(existingProduct, id, new PartitionKey(existingProduct.Category));

            try
            {
                var directoryClient = _shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient($"{id}.json");
                string jsonString = JsonSerializer.Serialize(existingProduct);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

                using (var stream = new MemoryStream(bytes))
                {
                    await fileClient.CreateAsync(stream.Length);
                    await fileClient.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Update Error: {ex.Message}");
            }

            return true;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var existingProduct = await GetProductByIdAsync(id);
            if (existingProduct == null) return false;

            await _container.DeleteItemAsync<Product>(id, new PartitionKey(existingProduct.Category));

            try
            {
                var directoryClient = _shareClient.GetRootDirectoryClient();
                var fileClient = directoryClient.GetFileClient($"{id}.json");
                await fileClient.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"File Delete Error: {ex.Message}");
            }

            return true;
        }

        public async Task<string> GetProductFileAsync(string id)
        {
            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient($"{id}.json");

            if (!await fileClient.ExistsAsync()) return null;

            var downloadInfo = await fileClient.DownloadAsync();
            using (var streamReader = new StreamReader(downloadInfo.Value.Content))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}