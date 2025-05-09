using Data_Organizer.DTOs;
using Data_Organizer.MVVM.Models;
using Data_Organizer.Services;

namespace Data_Organizer.Tests.Unit_Tests.Services
{
    public class MappingServiceTest
    {
        private readonly MappingService mappingService = new MappingService();

        [Fact]
        public void MapUser_Should_Return_Result()
        {
            var input = new User()
            {
                Uid = new Guid().ToString(),
                IsDeleted = false,
                IsMetadataStored = false
            };

            var result = mappingService.MapUser(input);

            Assert.NotNull(result);
            Assert.Equal(input.Uid, result.Uid);
            Assert.Null(result.UsersMetadataId);
            Assert.Equal(input.IsDeleted, result.IsDeleted);
            Assert.Equal(input.IsMetadataStored, result.IsMetadataStored);
        }

        [Fact]
        public void MapMetadata_Should_Return_Result()
        {
            var input = new UsersMetadata()
            {
                CreationDate = DateTime.Now,
                CreationDevice = null,
                CreationLocation = new MVVM.Models.Location() { Latitude = 10, Longitude = 10 },
                DeletionDate = DateTime.Now,
                DeletionDevice = null,
                DeletionLocation = new MVVM.Models.Location() { Longitude = 50, Latitude = 50 }
            };

            var result = mappingService.MapMetadata(input);

            Assert.NotNull(result);
            Assert.Equal(input.CreationDate, result.CreationDate);
            Assert.Null(result.CreationDeviceId);
            Assert.Equivalent(input.CreationLocation, result.CreationLocation);
            Assert.Equal(input.DeletionDate, result.DeletionDate);
            Assert.Null(result.DeletionDeviceId);
            Assert.Equivalent(input.DeletionLocation, result.DeletionLocation);
        }

        [Fact]
        public void MapNoteDTOToHeader_Should_Return_Result()
        {
            var input = new NoteDTO()
            {
                UserId = "UserId",
                NoteBodyId = "NoteBodyId",
                Title = "Title",
                PreviewText = "PreviewText",
                Content = "Content",
                CreationTime = DateTime.Now,
                IsDeleted = false,
                Error = "SomeError"
            };

            var result = mappingService.MapNoteDTOToHeader(input);

            Assert.NotNull(result);
            Assert.Equal(input.UserId, result.UserId);
            Assert.Equal(input.Title, result.Title);
            Assert.Equal(input.PreviewText, result.PreviewText);
            Assert.Equal(input.CreationTime, result.CreationTime);
            Assert.Equal(input.IsDeleted, result.IsDeleted);
            Assert.Equal(input.NoteBodyId, result.NoteBodyReferenceId);
        }

        [Fact]
        public void MapHeaderToNoteDTO_Should_Return_Result()
        {
            var input = new NoteHeader()
            {
                UserId = "UserId",
                NoteBodyReferenceId = "NoteBodyReferenceId",
                Title = "Title",
                PreviewText = "PreviewText",
                CreationTime = DateTime.Now,
                IsDeleted = false
            };

            var result = mappingService.MapHeaderToNoteDTO(input);

            Assert.NotNull(result);
            Assert.Equal(input.UserId, result.UserId);
            Assert.Equal(input.Title, result.Title);
            Assert.Equal(input.PreviewText, result.PreviewText);
            Assert.Equal(input.CreationTime, result.CreationTime);
            Assert.Equal(input.IsDeleted, result.IsDeleted);
            Assert.Equal(input.NoteBodyReferenceId, result.NoteBodyId);
        }
    }
}
