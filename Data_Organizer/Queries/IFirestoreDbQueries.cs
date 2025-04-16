using Data_Organizer.DTOs;
using Data_Organizer.Models;
using Refit;

namespace Data_Organizer.Queries
{
    public interface IFirestoreDbQueries
    {
        // Users
        [Post("/firestoredb/create-user")]
        Task<UserRequestDTO> CreateUserAsync([Body] UserRequestDTO request);

        [Get("/firestoredb/user/{uid}")]
        Task<User> GetUserByUidAsync(string uid);

        [Put("/firestoredb/update-user")]
        Task<User> UpdateUserAsync([Body] User user);

        [Delete("/firestoredb/remove-user")]
        Task<UserRequestDTO> RemoveUserAsync([Body] UserRequestDTO request);

        // Change password
        [Post("/firestoredb/create-change-password")]
        Task<ChangePassword> CreateChangePasswordAsync([Body] ChangePasswordRequestDTO request);

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
