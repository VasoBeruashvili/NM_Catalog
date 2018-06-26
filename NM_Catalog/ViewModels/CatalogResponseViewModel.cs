using Newtonsoft.Json;
using System.Collections.Generic;

namespace NM_Catalog.ViewModels
{
    public class CatalogResponseViewModel
    {
        [JsonProperty("products")]
        public List<ProductViewModel> Products { get; set; }

        [JsonProperty("totalItemsCount")]
        public int TotalItemsCount { get; set; }
    }
}