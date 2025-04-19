using System.Text.Json.Serialization;

namespace Data_Organizer.MVVM.Models
{
    public class DeviceInfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("idiom")]
        public string Idiom { get; set; }

        [JsonPropertyName("deviceType")]
        public string DeviceType { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("deviceInfoCombined")]
        public string DeviceInfoCombined => $"{Name}_{Model}_{Manufacturer}_{Platform}_{Idiom}_{DeviceType}";
    }
}
