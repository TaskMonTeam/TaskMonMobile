using ImportService.Client;
using TaskMonAdmin.Services;

namespace TaskMonAdmin
{
    public class TokenSupplierImport : ITokenSupplier
    {
        private readonly Auth0Service _auth0Service;

        public TokenSupplierImport(Auth0Service auth0Service)
        {
            _auth0Service = auth0Service;
        }
        
        public Task<string> GetTokenAsync()
        {
            return _auth0Service.GetAccessTokenAsync();
        }
    }
}