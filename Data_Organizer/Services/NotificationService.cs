﻿using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Data_Organizer.Interfaces;

namespace Data_Organizer.Services
{
    public class NotificationService : INotificationService
    {
        public async Task<string> ShowActionSheetAsync(string message = "Що ви бажаєте зробити?", params string[] actions)
        {
            var currentPage = Application.Current?.MainPage;
            return await currentPage.DisplayActionSheet(message, "Нічого", null, actions);
        }

        public async Task<bool> ShowConfirmationDialogAsync(string message)
        {
            var currentPage = Application.Current?.MainPage;
            return await currentPage.DisplayAlert("Підтвердження", message, "Так", "Ні");
        }

        public async Task ShowToastAsync(string message, double textSize = 20)
        {
            var notification = Toast.Make(message, ToastDuration.Long, textSize);
            await notification.Show();
        }
    }
}
