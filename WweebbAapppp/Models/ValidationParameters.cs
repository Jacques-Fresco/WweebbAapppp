using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WweebbAapppp.Models
{
    public static class ValidationParameters
    {
        public static TokenValidationParameters GetValidationParameters(IConfiguration configuration)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };

            return validationParameters;
        }
    }
}
