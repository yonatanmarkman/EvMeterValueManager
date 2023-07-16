using Newtonsoft.Json;

namespace EvMeterValueManager.Models
{
    public class MeterValueWithAnomaly : MeterValue
    {
        [JsonProperty("EnergyCorrectionValue")]
        public decimal EnergyCorrectionValue { get; set; }
    }
}
