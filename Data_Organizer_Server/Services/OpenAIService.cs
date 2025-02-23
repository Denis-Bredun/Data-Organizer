using Data_Organizer_Server.Interfaces;
using Data_Organizer_Server.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Message = Data_Organizer_Server.Models.Message;

namespace Data_Organizer_Server.Services
{
    public class OpenAIService : IOpenAIService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly List<Message> _messages;

        public const string ENDPOINT = "https://api.openai.com/v1/chat/completions";

        public OpenAIService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _messages = new List<Message>();
        }

        public async Task<string> GetSummary(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Пусті вхідні дані були відправлені на обробку ШІ!");

            if (_messages.Count == 0)
            {
                _messages.Add(new Message()
                {
                    Role = "system",
                    Content = "Ти асистент, що робить короткі тези для тексту, використовуючи мову, якою надано вхідні дані."
                });
            }

            var message = new Message() { Role = "user", Content = content };

            _messages.Add(message);

            var requestData = new Request()
            {
                ModelId = "gpt-4o-mini",
                Messages = _messages
            };

            var request = new HttpRequestMessage(HttpMethod.Post, ENDPOINT)
            {
                Content = new StringContent(JsonSerializer.Serialize(requestData))
            };

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _configuration.GetValue<string>("OPENAI_API_KEY"));

            using var response = await _httpClient.SendAsync(request);


            if (!response.IsSuccessStatusCode)
                throw new BadHttpRequestException($"{(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}");

            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

            var choices = responseData?.Choices ?? new List<Choice>();

            if (choices.Count == 0)
                throw new NullReferenceException("ШІ нічого не повернув!");

            var choice = choices[0];
            var responseMessage = choice.Message;

            _messages.Add(responseMessage);
            var responseText = responseMessage.Content.Trim();
            return responseText;
        }
    }
}
