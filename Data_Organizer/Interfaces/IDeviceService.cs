using Data_Organizer.Models;

namespace Data_Organizer.Interfaces
{
    public interface IDeviceService
    {
        Task<bool> RequestPermissionsLocationAsync();
        DeviceInfoModel GetDeviceInfo();
        Task<Data_Organizer.Models.Location> GetCurrentLocationAsync();
    }

}
