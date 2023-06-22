using System.ComponentModel.DataAnnotations;

namespace WweebbAapppp.Models
{
    public class TokensRequest
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
