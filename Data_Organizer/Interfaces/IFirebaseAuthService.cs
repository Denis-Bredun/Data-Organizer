namespace Data_Organizer.Interfaces
{
    public interface IFirebaseAuthService
    {
        Task<bool> SignUpAsync(string email, string password, string username = "");
        Task<bool> SignInAsync(string email, string password);
        Task<string?> GetFreshToken();
    }
}
