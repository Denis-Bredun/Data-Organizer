using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IDeviceService
    {
        Task<bool> RequestPermissionsLocationAsync();
        DeviceInfoModel GetDeviceInfo();
        Task<Data_Organizer.MVVM.Models.Location> GetCurrentLocationAsync();
    }

}
