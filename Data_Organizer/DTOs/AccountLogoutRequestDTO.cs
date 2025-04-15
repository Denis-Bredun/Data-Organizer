using Data_Organizer_Server.Models;

namespace Data_Organizer.DTOs
{
    public class AccountLogoutCreationRequest
    {
        public string UserId { get; set; }
        public AccountLogout AccountLogout { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
