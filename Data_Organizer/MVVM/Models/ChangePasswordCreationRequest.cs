namespace Data_Organizer_Server.Models
{
    public class ChangePasswordCreationRequest
    {
        public string Uid { get; set; }
        public ChangePassword ChangePassword { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
