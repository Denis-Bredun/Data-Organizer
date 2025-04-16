using Data_Organizer.DTOs;
using Data_Organizer.Interfaces;
using Data_Organizer.Models;

namespace Data_Organizer.Services
{
    public class MappingService : IMappingService
    {
        public UserDTO MapUser(User user)
        {
            return new UserDTO
            {
                Uid = user.Uid,
                UsersMetadataId = user.UsersMetadata?.Id,
                IsDeleted = user.IsDeleted,
                IsMetadataStored = user.IsMetadataStored
            };
        }

        public UsersMetadataDTO MapMetadata(UsersMetadata metadata)
        {
            return new UsersMetadataDTO
            {
                CreationDate = metadata.CreationDate,
                CreationDeviceId = metadata.CreationDevice?.Id,
                CreationLocation = metadata.CreationLocation,
                DeletionDate = metadata.DeletionDate,
                DeletionDeviceId = metadata.DeletionDevice?.Id,
                DeletionLocation = metadata.DeletionLocation
            };
        }
    }

}
