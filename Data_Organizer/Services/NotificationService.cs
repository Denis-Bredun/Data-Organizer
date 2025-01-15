using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class NotificationService : INotificationService
    {
        public async Task<string> ShowActionSheetAsync(params string[] actions)
        {
            var currentPage = Application.Current?.MainPage;
            return await currentPage.DisplayActionSheet("Що ви бажаєте зробити?", "Нічого", null, actions);
        }

        public async Task<bool> ShowConfirmationDialogAsync(string message)
        {
            var currentPage = Application.Current?.MainPage;
            return await currentPage.DisplayAlert("Підтвердження", message, "Так", "Ні");
        }

        public async Task ShowToastAsync(string message)
        {
            var notification = Toast.Make(message, ToastDuration.Long, 20);
            await notification.Show();
        }
    }
}
