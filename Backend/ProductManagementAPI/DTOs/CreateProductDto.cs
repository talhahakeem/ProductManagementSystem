namespace ProductManagementAPI.DTOs
{
    public class CreateProductDto
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}