using CommunityToolkit.Maui.Storage;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models.Enums;
using DocumentFormat.OpenXml.Packaging;
using iText.Kernel.Pdf;
using iText.Layout.Element;
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
                            "text/plain",
                            "application/rtf",
                            "text/rtf",
                            "application/pdf",
                            "application/msword",
                            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                            "text/csv",
                            "text/xml",
                            "application/json",
                            "text/html",
                            "application/x-subrip",
                            "text/x-vcard",
                            "application/x-yaml",
                            "application/x-markdown",
                            ".rtf"
                        }
                    }
                });

            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Оберіть текстовий файл"
            });

            if (result == null) return null;

            if (result.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                await using var stream = await result.OpenReadAsync();
                return ExtractTextFromPdf(stream);
            }

            if (result.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                using var stream = await result.OpenReadAsync();
                using var doc = WordprocessingDocument.Open(stream, false);
                return doc.MainDocumentPart?.Document.Body?.InnerText;
            }

            if (result.FileName.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
            {
                await using var stream = await result.OpenReadAsync();
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }

            await using var textStream = await result.OpenReadAsync();
            using var textReader = new StreamReader(textStream);
            return await textReader.ReadToEndAsync();
        }

        private string ExtractTextFromPdf(Stream pdfStream)
        {
            var text = new StringBuilder();

            using (var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(pdfStream))
            {
                foreach (var page in pdfDocument.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
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
                    byte[] pdfBytes;
                    using (var pdfStream = new MemoryStream())
                    {
                        using (var writer = new PdfWriter(pdfStream))
                        using (var pdf = new PdfDocument(writer))
                        using (var document = new iText.Layout.Document(pdf))
                        {
                            document.Add(new Paragraph(text));
                        }

                        pdfBytes = pdfStream.ToArray();
                    }

                    await stream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
                    stream.Position = 0;
                    break;
            }

            stream.Position = 0;
            var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream);

            if (!fileSaverResult.IsSuccessful)
                throw fileSaverResult.Exception ?? new Exception("Не вдалося зберігти файл(");
        }
    }
}
