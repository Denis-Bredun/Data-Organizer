
using Data_Organizer.DTOs;

namespace Data_Organizer.Interfaces
{
    public interface IFirestoreDbService
    {
        Task CreateUserAsync(bool isMetadataStored, Data_Organizer.Models.Location location);
        Task<bool> GetUserMetadataFlagAsync();
        Task<UsersMetadataDTO> GetUsersMetadataDTO(string uid);
        Task RemoveUserAsync(string uid, bool isMetadataStored, Models.Location location);
        Task SetUserMetadataFlagAsync(bool isMetadataStored);
    }
}
