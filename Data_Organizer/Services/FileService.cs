using CommunityToolkit.Maui.Storage;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models.Enums;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
using Xceed.Words.NET;

namespace Data_Organizer.Services
{
    public class FileService : IFileService
    {
        public async Task<bool> RequestPermissionsStorageReadAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            return status == PermissionStatus.Granted;
        }

        public async Task<bool> RequestPermissionsStorageWriteAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            return status == PermissionStatus.Granted;
        }

        public async Task<string> ImportTextAsync()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {
                        DevicePlatform.Android, new[]
                        {
                            "text/plain", // .txt
                            "application/pdf", // .pdf
                            "application/msword", // .doc
                            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // .docx
                            "application/rtf", // .rtf
                            "text/csv", // .csv
                            "text/xml", // .xml
                            "application/json", // .json
                            "text/html", // .html
                            "application/x-subrip", // .srt
                            "text/x-vcard", // .vcf
                            "application/x-yaml", // .yaml, .yml
                            "application/x-markdown" // .md
                        }
                    }
                });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Оберіть текстовий файл"
            });

            if (result == null) return null;

            await using var stream = await result.OpenReadAsync();
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task ExportTextAsync(string text, TextFileFormat textFileFormat)
        {
            var fileName = $"document.{textFileFormat.ToString().ToLower()}";
            using var stream = new MemoryStream();

            switch (textFileFormat)
            {
                case TextFileFormat.TXT:
                    stream.Write(Encoding.UTF8.GetBytes(text));
                    break;

                case TextFileFormat.DOCX:
                    using (var doc = DocX.Create(stream))
                    {
                        doc.InsertParagraph(text);
                        doc.Save();
                    }
                    break;

                case TextFileFormat.PDF:
                    Settings.License = LicenseType.Community;
                    var document = Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);
                            page.Content().Text(text);
                        });
                    });
                    document.GeneratePdf(stream);
                    break;

                case TextFileFormat.RTF:
                    var rtfContent = @"{\rtf1\ansi " + text.Replace("\n", @"\par ") + "}";
                    stream.Write(Encoding.ASCII.GetBytes(rtfContent));
                    break;
            }

            stream.Position = 0;
            var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream);

            if (!fileSaverResult.IsSuccessful)
                throw fileSaverResult.Exception ?? new Exception("Не вдалося зберігти файл(");
        }
    }
}
