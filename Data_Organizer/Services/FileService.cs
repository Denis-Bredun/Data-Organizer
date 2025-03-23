using CommunityToolkit.Maui.Storage;
using Data_Organizer.APIRequestTools;
using Data_Organizer.Interfaces;
using Data_Organizer.MVVM.Models;
using Data_Organizer.MVVM.Models.Enums;
using DocumentFormat.OpenXml.Packaging;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Refit;
using System.Text;
using Xceed.Words.NET;

namespace Data_Organizer.Services
{
    public class FileService : IFileService
    {
        private readonly IGetTranscriptionFromAudiofileQuery _getTranscriptionFromAudiofileQuery;

        public FileService(IGetTranscriptionFromAudiofileQuery getTranscriptionFromAudiofileQuery)
        {
            _getTranscriptionFromAudiofileQuery = getTranscriptionFromAudiofileQuery;
        }

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
            var result = await PickTextFileAsync();
            if (result == null) return null;

            return await ReadTextFromFileAsync(result);
        }

        private async Task<FileResult?> PickTextFileAsync()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {
                        DevicePlatform.Android, new[]
                        {
                            "text/plain", "application/rtf", "text/rtf", "application/pdf",
                            "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                            "text/csv", "text/xml", "application/json", "text/html",
                            "application/x-subrip", "text/x-vcard", "application/x-yaml", "application/x-markdown", ".rtf"
                        }
                    }
                });

            return await FilePicker.Default.PickAsync(new PickOptions
            {
                FileTypes = customFileType,
                PickerTitle = "Оберіть текстовий файл"
            });
        }

        private async Task<string> ReadTextFromFileAsync(FileResult result)
        {
            await using var stream = await result.OpenReadAsync();

            return result.FileName.ToLower() switch
            {
                var name when name.EndsWith(".pdf") => ExtractTextFromPdf(stream),
                var name when name.EndsWith(".docx") => ExtractTextFromDocx(stream),
                _ => await ReadTextStreamAsync(stream)
            };
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

        private string ExtractTextFromDocx(Stream stream)
        {
            using var doc = WordprocessingDocument.Open(stream, false);
            return doc.MainDocumentPart?.Document.Body?.InnerText ?? string.Empty;
        }

        private async Task<string> ReadTextStreamAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task ExportTextAsync(string text, TextFileFormat textFileFormat)
        {
            var fileName = $"document.{textFileFormat.ToString().ToLower()}";
            using var stream = new MemoryStream();

            await WriteTextToStreamAsync(stream, text, textFileFormat);

            stream.Position = 0;
            var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream);

            if (!fileSaverResult.IsSuccessful)
                throw fileSaverResult.Exception ?? new Exception("Не вдалося зберігти файл(");
        }

        private async Task WriteTextToStreamAsync(MemoryStream stream, string text, TextFileFormat format)
        {
            switch (format)
            {
                case TextFileFormat.TXT:
                    stream.Write(Encoding.UTF8.GetBytes(text));
                    break;

                case TextFileFormat.DOCX:
                    WriteDocxToStream(stream, text);
                    break;

                case TextFileFormat.PDF:
                    await WritePdfToStreamAsync(stream, text);
                    break;
            }
        }

        private void WriteDocxToStream(Stream stream, string text)
        {
            using var doc = DocX.Create(stream);
            doc.InsertParagraph(text);
            doc.Save();
        }

        private async Task WritePdfToStreamAsync(Stream stream, string text)
        {
            byte[] pdfBytes;
            using (var pdfStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(pdfStream))
                using (var pdf = new PdfDocument(writer))
                using (var document = new iText.Layout.Document(pdf))
                {
                    var fontData = await LoadFontFromResources("arialuni.ttf");

                    PdfFont font = PdfFontFactory.CreateFont(
                        fontData,
                        PdfEncodings.IDENTITY_H,
                        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

                    document.SetFont(font);
                    document.Add(new Paragraph(text).SetFont(font));
                }

                pdfBytes = pdfStream.ToArray();
            }

            await stream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
            stream.Position = 0;
        }

        private async Task<byte[]> LoadFontFromResources(string fontFileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fontFileName);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<string> ImportAudiofileAsync(LanguageModel selectedLanguage)
        {
            var result = await PickAudioFileAsync();
            if (result == null) return null;

            return await TranscribeAudiofile(result, selectedLanguage.CultureCode);
        }

        private async Task<FileResult> PickAudioFileAsync()
        {
            var customFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/*" } }
                });

            var options = new PickOptions
            {
                PickerTitle = "Будь ласка, виберіть аудіофайл",
                FileTypes = customFileType,
            };

            return await FilePicker.PickAsync(options);
        }

        private async Task<string> TranscribeAudiofile(FileResult fileResult, string languageCode)
        {
            Stream fileStream = null;
            try
            {
                fileStream = await fileResult.OpenReadAsync();
                var streamPart = new StreamPart(fileStream, fileResult.FileName, fileResult.ContentType);

                var response = await _getTranscriptionFromAudiofileQuery.Execute(streamPart, languageCode);

                if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    throw new Exception(response.Error);
                }

                return response.Transcription;
            }
            catch (ApiException apiEx)
            {
                if (apiEx.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    string errorMessage = await apiEx.GetContentAsAsync<string>();
                    if (errorMessage.Contains("File size exceeds"))
                        throw new Exception("Розмір файлу перевищує допустимий ліміт 100 МБ.", apiEx);
                    if (errorMessage.Contains("Please upload an audio file"))
                        throw new Exception("Будь ласка, завантажте аудіофайл.", apiEx);

                    throw new Exception("Помилка запиту: " + errorMessage, apiEx);
                }
                throw new Exception("Помилка запиту до сервера: " + apiEx.Message, apiEx);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Помилка підключення до сервісу розпізнавання мови. Спробуйте пізніше.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Помилка конфігурації сервісу. Зверніться до адміністратора.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Некоректні дані для транскрипції: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Сталася помилка при обробці запиту. Спробуйте пізніше.", ex);
            }
            finally
            {
                if (fileStream != null)
                {
                    await fileStream.DisposeAsync();
                }
            }
        }
    }
}
