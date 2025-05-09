using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Services
{
    public class DeviceServiceDecorator : IDeviceServiceDecorator
    {
        private readonly IDeviceService _deviceService;
        private readonly INotificationService _notificationService;

        public DeviceServiceDecorator(
            IDeviceService deviceService,
            INotificationService notificationService)
        {
            _deviceService = deviceService;
            _notificationService = notificationService;
        }

        public DeviceInfoModel GetDeviceInfo()
        {
            try
            {
                return _deviceService.GetDeviceInfo();
            }
            catch (Exception ex)
            {
                _notificationService.ShowToastAsync($"Помилка отримання даних пристрою: {ex.Message}");
                return null;
            }
        }

        public async Task<Data_Organizer.MVVM.Models.Location> GetCurrentLocationAsync()
        {
            try
            {
                if (!await CheckInternetConnectionAsync())
                    return null;

                if (!await _deviceService.RequestPermissionsLocationAsync())
                {
                    await _notificationService.ShowToastAsync("Доступ до геолокації не надано");
                    return null;
                }

                var location = await _deviceService.GetCurrentLocationAsync();

                if (location != null)
                    await _notificationService.ShowToastAsync("Локація успішно отримана");

                return location;
            }
            catch (FeatureNotSupportedException)
            {
                await _notificationService.ShowToastAsync("Геолокація не підтримується на цьому пристрої");
                return null;
            }
            catch (FeatureNotEnabledException)
            {
                await _notificationService.ShowToastAsync("Геолокація вимкнена. Увімкніть її в налаштуваннях.");
                return null;
            }
            catch (Exception ex)
            {
                await _notificationService.ShowToastAsync($"Помилка геолокації: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> CheckInternetConnectionAsync()
        {
            var isConnected = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            if (!isConnected)
                await _notificationService.ShowToastAsync("Немає Інтернет-підключення");

            return isConnected;
        }
    }

}
