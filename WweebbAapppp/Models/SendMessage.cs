using System.ComponentModel.DataAnnotations;

namespace WweebbAapppp.Models
{
    public class SendMessage
    {
        [Required]
        public string token { get; set; }
        [Required]
        public string message { get; set; }
    }
}
