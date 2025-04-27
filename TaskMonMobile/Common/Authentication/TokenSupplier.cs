using SurveyService.Models;
using TaskMonMobile.Services;

namespace TaskMonMobile
{
    public class TokenSupplier : ITokenSupplier
    {
        private readonly Auth0Service _auth0Service;

        public TokenSupplier(Auth0Service auth0Service)
        {
            _auth0Service = auth0Service;
        }
        
        public Task<string> GetTokenAsync()
        {
            return _auth0Service.GetAccessTokenAsync();
        }
    }
}