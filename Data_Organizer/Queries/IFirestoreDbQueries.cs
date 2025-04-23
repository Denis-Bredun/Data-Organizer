using Data_Organizer.DTOs;
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

        [Put("/firestoredb/set-metadata-flag")]
        Task<UserMetadataFlagUpdateDTO> SetUserMetadataFlagAsync([Body] UserMetadataFlagUpdateDTO updateDTO);

        // Change password
        [Post("/firestoredb/create-change-password")]
        Task<ChangePasswordRequestDTO> CreateChangePasswordAsync([Body] ChangePasswordRequestDTO request);

        // Account login/logout
        [Post("/firestoredb/create-account-login")]
        Task<AccountLoginRequestDTO> CreateAccountLoginAsync([Body] AccountLoginRequestDTO request);

        [Post("/firestoredb/create-account-logout")]
        Task<AccountLogoutRequestDTO> CreateAccountLogoutAsync([Body] AccountLogoutRequestDTO request);

        // Notes
        [Post("/firestoredb/create-note")]
        Task<NoteDTO> CreateNoteAsync([Body] NoteDTO note);

        [Post("/firestoredb/headers")]
        Task<List<NoteDTO>> GetNoteHeadersByUidAsync(UserDTO userDTO);

        [Post("/firestoredb/body-by-header")]
        Task<NoteDTO> GetNoteBodyByHeaderAsync([Body] NoteDTO noteDTO);

        [Delete("/firestoredb/remove-note")]
        Task<NoteDTO> RemoveNoteAsync([Body] NoteDTO noteDTO);

        [Put("/firestoredb/update-note")]
        Task<NoteDTO> UpdateNoteAsync([Body] NoteDTO request);
    }
}
