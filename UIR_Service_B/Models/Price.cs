using System.Text.Json.Serialization;

namespace UIR_Service_B.Models
{
    public class Price
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cabinetId")]
        public int CabinetId { get; set; }

        [JsonPropertyName("hour")]
        public int Hour { get; set; }

        [JsonPropertyName("price")]
        public decimal price { get; set; }

        [JsonPropertyName("cancelTime")]
        public int CancelTime { get; set; }

        [JsonPropertyName("createdAt")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public string UpdatedAt { get; set; }
    }
}
