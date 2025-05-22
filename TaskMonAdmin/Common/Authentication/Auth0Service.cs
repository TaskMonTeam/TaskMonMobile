using Auth0.OidcClient;
using Microsoft.Extensions.Logging;

namespace TaskMonAdmin.Services
{
    public class Auth0Service
    {
        private readonly Auth0Client _auth0Client;
        private readonly ILogger<Auth0Service> _logger;
        private readonly ISecureStorage _secureStorage;
        private const string AccessTokenKey = "auth0_access_token";

        public Auth0Service(Auth0Client auth0Client, ISecureStorage secureStorage, ILogger<Auth0Service> logger)
        {
            _auth0Client = auth0Client;
            _secureStorage = secureStorage;
            _logger = logger;
        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                var loginResult = await _auth0Client.LoginAsync(new
                {
                    audience = "https://taskmon.com"
                });

                if (loginResult.IsError)
                {
                    _logger.LogError($"Не вийшло залогінитись: {loginResult.Error}");
                    return false;
                }
                
                await _secureStorage.SetAsync(AccessTokenKey, loginResult.AccessToken);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка під час логіну");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                _secureStorage.Remove(AccessTokenKey);
                
                await _auth0Client.LogoutAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка під час виходу з акаунту");
                return false;
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _secureStorage.GetAsync(AccessTokenKey);
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetAccessTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}