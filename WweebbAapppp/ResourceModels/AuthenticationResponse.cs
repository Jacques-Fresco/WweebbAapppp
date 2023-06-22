using System.ComponentModel.DataAnnotations;

namespace WweebbAapppp.ResourceModels
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public DateTime expirationAccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime expirationRefreshToken { get; set; }
    }
}
