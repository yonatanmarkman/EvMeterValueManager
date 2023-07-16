using Newtonsoft.Json;

namespace EvMeterValueManager.Models
{
    public class MeterValue
    {
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("ChargerId")]
        public long ChargerId { get; set; }

        [JsonProperty("Energy")]
        public decimal Energy { get; set; }

        [JsonProperty("ConnectorId")]
        public short ConnectorId { get; set; }
    }
}
