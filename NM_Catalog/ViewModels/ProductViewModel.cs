using Newtonsoft.Json;
using System.Collections.Generic;

namespace NM_Catalog.ViewModels
{
    public class ProductViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("isActive")]
        public string IsActive { get; set; }

        [JsonProperty("subCategoryId")]
        public int SubCategoryId { get; set; }

        [JsonProperty("quantityToAdd")]
        public int QuantityToAdd { get; set; }

        [JsonProperty("sum")]
        public decimal Sum { get; set; }

        [JsonProperty("productImage")]
        public string ProductImage { get; set; }
    }
}