namespace Data_Organizer.Interfaces
{
    public interface INotificationService
    {
        Task ShowToastAsync(string message, double textSize = 20);
        Task<bool> ShowConfirmationDialogAsync(string message);
        Task<string> ShowActionSheetAsync(string message, params string[] actions);
    }
}
