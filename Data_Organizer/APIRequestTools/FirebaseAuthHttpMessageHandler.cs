using Data_Organizer.Interfaces;
using System.Net.Http.Headers;

namespace Data_Organizer.APIRequestTools
{
    public class FirebaseAuthHttpMessageHandler : DelegatingHandler
    {
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        public FirebaseAuthHttpMessageHandler(IGoogleAuthenticationService googleAuthenticationService)
        {
            _googleAuthenticationService = googleAuthenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? firebaseToken = await _googleAuthenticationService.GetFreshToken();

            if (firebaseToken != null)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", firebaseToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
