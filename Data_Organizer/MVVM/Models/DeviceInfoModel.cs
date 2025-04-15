namespace Data_Organizer_Server.Models
{
    public class DeviceInfoModel
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        public string DeviceType { get; set; }
        public string Version { get; set; }
        public string DeviceInfoCombined => $"{Name}_{Model}_{Manufacturer}_{Platform}_{Idiom}_{DeviceType}";
    }
}
