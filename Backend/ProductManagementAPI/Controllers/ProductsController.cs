using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Interfaces;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // 1. CREATE
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDto dto)
        {
            var newProduct = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        // 2. READ ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // 3. READ BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }

        // 4. UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto dto)
        {
            var success = await _productService.UpdateProductAsync(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }

        // 5. DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        // 6. GET FILE
        [HttpGet("{id}/file")]
        public async Task<IActionResult> GetProductFile(string id)
        {
            var fileContent = await _productService.GetProductFileAsync(id);
            if (fileContent == null) return NotFound(new { message = "JSON file not found in Azure File Storage." });

            return Content(fileContent, "application/json");
        }
    }
}