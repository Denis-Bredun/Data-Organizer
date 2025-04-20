
using Data_Organizer.DTOs;

namespace Data_Organizer.Interfaces
{
    public interface IFirestoreDbService
    {
        Task CreateUserAsync(bool isMetadataStored, Data_Organizer.MVVM.Models.Location location);
        Task<bool> GetUserMetadataFlagAsync();
        Task<UsersMetadataDTO> GetUsersMetadataDTO(string uid);
        Task RemoveUserAsync(string uid, bool isMetadataStored, Data_Organizer.MVVM.Models.Location location);
        Task CreateChangePasswordInstance(string oldPassword);
        Task SetUserMetadataFlagAsync(bool isMetadataStored);
        Task CreateAccountLoginInstance();
        Task CreateAccountLogoutInstance();
        Task CreateNoteAsync(string content);
    }
}
