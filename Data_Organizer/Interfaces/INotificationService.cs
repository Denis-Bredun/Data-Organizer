namespace Data_Organizer.Interfaces
{
    public interface INotificationService
    {
        Task ShowToastAsync(string message);
        Task<bool> ShowConfirmationDialogAsync(string message);
        Task<string> ShowActionSheetAsync(string message, params string[] actions);
    }
}
