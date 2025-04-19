using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Services
{
    public class DeviceService : IDeviceService
    {
        public async Task<bool> RequestPermissionsLocationAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted;
        }

        public DeviceInfoModel GetDeviceInfo()
        {
            return new DeviceInfoModel
            {
                Name = DeviceInfo.Name,
                Model = DeviceInfo.Model,
                Manufacturer = DeviceInfo.Manufacturer,
                Platform = DeviceInfo.Platform.ToString(),
                Idiom = DeviceInfo.Idiom.ToString(),
                DeviceType = DeviceInfo.DeviceType.ToString(),
                Version = DeviceInfo.VersionString
            };
        }

        public async Task<Data_Organizer.MVVM.Models.Location> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.Default.GetLocationAsync(request);

                return location != null
                    ? new Data_Organizer.MVVM.Models.Location { Latitude = location.Latitude, Longitude = location.Longitude }
                    : null;
            }
            catch (Exception ex)
            {
                throw WrapException(ex);
            }
        }

        private Exception WrapException(Exception ex)
        {
            return ex switch
            {
                FeatureNotSupportedException =>
                    new Exception("Геолокація не підтримується на цьому пристрої.", ex),
                FeatureNotEnabledException =>
                    new Exception("Геолокація вимкнена. Увімкніть її в налаштуваннях.", ex),
                PermissionException =>
                    new Exception("Доступ до геолокації не надано. Надайте дозвіл.", ex),
                _ => new Exception("Помилка отримання геолокації.", ex)
            };
        }
    }
}
