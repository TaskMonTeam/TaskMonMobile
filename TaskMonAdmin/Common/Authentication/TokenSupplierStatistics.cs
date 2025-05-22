using StatisticsService.Client;
using TaskMonAdmin.Services;

namespace TaskMonAdmin
{
    public class TokenSupplierStatistics : ITokenSupplier
    {
        private readonly Auth0Service _auth0Service;

        public TokenSupplierStatistics(Auth0Service auth0Service)
        {
            _auth0Service = auth0Service;
        }
        
        public Task<string> GetTokenAsync()
        {
            return _auth0Service.GetAccessTokenAsync();
        }
    }
}