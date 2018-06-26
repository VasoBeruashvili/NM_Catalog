using Newtonsoft.Json;

namespace NM_Catalog.ViewModels
{
    public class SubCategoryViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }
    }
}