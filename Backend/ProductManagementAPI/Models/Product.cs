using System.Text.Json.Serialization; 
using Newtonsoft.Json;

namespace ProductManagementAPI.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}