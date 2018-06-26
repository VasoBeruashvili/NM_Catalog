using Newtonsoft.Json;
using System.Collections.Generic;

namespace NM_Catalog.ViewModels
{
    public class OrderViewModel
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}