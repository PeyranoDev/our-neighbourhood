using Application.Providers;
using Application.Services.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers
{
    public class FirebaseProvider : INotificationSender
    {
        private static FirebaseApp? _firebaseApp;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IConfiguration _configuration;

        public FirebaseProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string token, string title, string body, Dictionary<string, string> data)
        {
            var firebaseApp = await GetAppAsync();
            var message = new Message
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data
            };

            await FirebaseMessaging.GetMessaging(firebaseApp).SendAsync(message);
        }

        private async Task<FirebaseApp> GetAppAsync()
        {
            if (_firebaseApp != null) return _firebaseApp;

            await _semaphore.WaitAsync();
            try
            {
                if (_firebaseApp == null)
                {
                    var jsonCredentials = _configuration["FirebaseServiceAccount"];
                    if (string.IsNullOrEmpty(jsonCredentials))
                        throw new InvalidOperationException("El secreto 'FirebaseServiceAccount' no se encontró.");

                    _firebaseApp = FirebaseApp.Create(new AppOptions
                    {
                        Credential = GoogleCredential.FromJson(jsonCredentials)
                    });
                }
            }
            finally
            {
                _semaphore.Release();
            }

            return _firebaseApp!;
        }
    }
}
