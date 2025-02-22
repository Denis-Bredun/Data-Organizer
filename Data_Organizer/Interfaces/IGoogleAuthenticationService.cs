namespace Data_Organizer.Interfaces
{
    public interface IGoogleAuthenticationService
    {
        Task<bool> SignUpAsync(string email, string password, string username = "");
        Task<bool> SignInAsync(string email, string password);
    }
}
