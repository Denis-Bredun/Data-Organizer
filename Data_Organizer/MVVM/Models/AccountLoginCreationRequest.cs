namespace Data_Organizer_Server.Models
{
    public class AccountLoginCreationRequest
    {
        public string UserId { get; set; }
        public AccountLogin AccountLogin { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
