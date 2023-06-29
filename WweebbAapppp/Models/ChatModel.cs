using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace NamespaceChat
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        //public ICollection<User> Participants { get; set; }
        //public ICollection<Message> Messages { get; set; }
    }

/*    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Message> Messages { get; set; }
    }*/

    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }
        public string UserId { get; set; }
        //public User User { get; set; }
        public int ChatId { get; set; }
        //public Chat Chat { get; set; }
    }
}