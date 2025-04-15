using Data_Organizer_Server.Models;

namespace Data_Organizer.DTOs
{
    public class ChangePasswordRequestDTO
    {
        public string Uid { get; set; }
        public ChangePassword ChangePassword { get; set; }
        public DeviceInfoModel DeviceInfo { get; set; }
    }
}
