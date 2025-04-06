namespace Data_Organizer.Interfaces
{
    public interface IFirebaseAuthService
    {
        event EventHandler AuthStateChanged;

        Task<bool> SignUpAsync(string email, string password, string username = "");
        Task<bool> SignInAsync(string email, string password);
        Task<string?> GetFreshToken();
        bool IsUserAuthorized();
        string GetUsername();
        Task<bool> SignOut();
        Task<bool> ResetPassword(string email);
        Task<bool> SignOutWithoutNotification();
    }
}
