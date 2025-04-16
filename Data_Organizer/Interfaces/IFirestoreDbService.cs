
namespace Data_Organizer.Interfaces
{
    public interface IFirestoreDbService
    {
        Task CreateUserAsync(bool isMetadataStored, Data_Organizer.Models.Location location);
    }
}
