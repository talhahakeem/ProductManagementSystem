using ProductManagementAPI.Models;
using ProductManagementAPI.DTOs;

namespace ProductManagementAPI.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto dto);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<bool> UpdateProductAsync(string id, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(string id);
        Task<string> GetProductFileAsync(string id);
    }
}