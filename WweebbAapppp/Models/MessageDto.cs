using System.ComponentModel.DataAnnotations;

namespace WweebbAapppp.Models
{
    public class MessageDto
    {
        /*[Required]
        public int Id { get; set; }*/
        [Required]
        public string? message { get; set; }
        [Required]
        public string? senderid { get; set; }
        [Required]
        public int chatid { get; set; }
    }
}
