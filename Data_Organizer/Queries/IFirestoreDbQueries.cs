using Data_Organizer.DTOs;
using Data_Organizer.MVVM.Models;
using Refit;

namespace Data_Organizer.Queries
{
    public interface IFirestoreDbQueries
    {
        // Users
        [Post("/firestoredb/create-user")]
        Task<UserRequestDTO> CreateUserAsync([Body] UserRequestDTO request);

        [Delete("/firestoredb/remove-user")]
        Task<UserRequestDTO> RemoveUserAsync([Body] UserRequestDTO request);

        // Metadata
        [Post("/firestoredb/get-metadata")]
        Task<UsersMetadataDTO> GetUserMetadataAsync([Body] UsersMetadataDTO request);

        [Post("/firestoredb/get-metadata-flag")]
        Task<UserMetadataFlagUpdateDTO> GetUserMetadataFlagAsync([Body] UserMetadataFlagUpdateDTO request);

        [Post("/firestoredb/set-metadata-flag")]
        Task<UserMetadataFlagUpdateDTO> SetUserMetadataFlagAsync([Body] UserMetadataFlagUpdateDTO updateDTO);

        // Change password
        [Post("/firestoredb/create-change-password")]
        Task<ChangePasswordRequestDTO> CreateChangePasswordAsync([Body] ChangePasswordRequestDTO request);

        // Account login/logout
        [Post("/firestoredb/create-account-login")]
        Task<AccountLogin> CreateAccountLoginAsync([Body] AccountLoginRequestDTO request);

        [Post("/firestoredb/create-account-logout")]
        Task<AccountLogout> CreateAccountLogoutAsync([Body] AccountLogoutRequestDTO request);

        // Notes
        [Post("/firestoredb/create-note")]
        Task<Note> CreateNoteAsync([Body] Note note);

        [Get("/firestoredb/headers/{uid}")]
        Task<List<NoteHeader>> GetNoteHeadersByUidAsync(string uid);

        [Post("/firestoredb/body-by-header")]
        Task<NoteBody> GetNoteBodyByHeaderAsync([Body] NoteHeader noteHeader);

        [Delete("/firestoredb/remove-note")]
        Task<NoteHeader> RemoveNoteAsync([Body] NoteHeader noteHeader);

        [Put("/firestoredb/update-note")]
        Task<Note> UpdateNoteAsync([Body] Note note);
    }

}
