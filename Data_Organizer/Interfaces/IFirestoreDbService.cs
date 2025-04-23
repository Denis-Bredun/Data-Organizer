
using Data_Organizer.DTOs;
using Data_Organizer.MVVM.Models;

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
        Task<List<NoteHeader>> GetNoteHeadersByUidAsync();
        Task<bool> RemoveNoteAsync(NoteHeader header);
        Task<NoteBody> GetNoteBodyByHeaderAsync(NoteHeader header);
        Task<bool> UpdateNoteAsync(NoteHeader header, NoteBody body);
    }
}
