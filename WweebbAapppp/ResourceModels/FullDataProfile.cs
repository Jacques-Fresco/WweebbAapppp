using System.ComponentModel.DataAnnotations;

namespace VehicleQuotes.ResourceModels
{
    public class FullDataProfileDTO : User
    {
        public string Id { get; set; }
        public string PasswordHash { get; set; }
    }
}
