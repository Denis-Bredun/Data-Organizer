using Data_Organizer_Server.Models;

namespace Data_Organizer.Interfaces
{
    public interface IDeviceServiceDecorator
    {
        DeviceInfoModel GetDeviceInfo();
        Task<Data_Organizer_Server.Models.Location> GetCurrentLocationAsync();
    }
}
