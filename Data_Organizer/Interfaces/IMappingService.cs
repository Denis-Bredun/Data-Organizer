using Data_Organizer.DTOs;
using Data_Organizer.MVVM.Models;

namespace Data_Organizer.Interfaces
{
    public interface IMappingService
    {
        UserDTO MapUser(User user);
        UsersMetadataDTO MapMetadata(UsersMetadata metadata);
        NoteHeader MapNoteDTOToHeader(NoteDTO dto);
        NoteDTO MapHeaderToNoteDTO(NoteHeader header);
    }
}
