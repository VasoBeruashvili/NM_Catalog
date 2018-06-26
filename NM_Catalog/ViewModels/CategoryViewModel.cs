using Newtonsoft.Json;
using System.Collections.Generic;

namespace NM_Catalog.ViewModels
{
    public class CategoryViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fontawesomeIcon")]
        public string FontawesomeIcon { get; set; }

        [JsonProperty("subCategories")]
        public List<SubCategoryViewModel> SubCategories { get; set; }
    }
}