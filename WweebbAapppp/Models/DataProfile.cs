using System.ComponentModel.DataAnnotations;

namespace WweebbAapppp.Models
{
    public class DataProfile
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
