using Data_Organizer_Server.Models;

namespace Data_Organizer.DTOs
{
    public class AccountLoginCreationRequest
    {
        public string UserId { get; set; }
        public AccountLogin AccountLogin { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
