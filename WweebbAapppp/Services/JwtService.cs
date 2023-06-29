using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WweebbAapppp.Models;
using WweebbAapppp.ResourceModels;

namespace WweebbAapppp.Services
{
    public class JwtService
    {
        public const int EXPIRATION_MINUTES = 1;
        public const int EXPIRATION_MINUTES_REFRESH_TOKEN = 20;

        private readonly IConfiguration _configuration;
        private readonly RefreshTokenService _refreshTokenService;

        public JwtService(IConfiguration configuration, RefreshTokenService refreshTokenService)
        {
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        public AuthenticationResponse CreateToken(IdentityUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);
            var expiration_refreshToken = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES_REFRESH_TOKEN);


            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );

            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            _refreshTokenService.SaveRefreshToken(user.Id, refreshToken, expiration_refreshToken);

            var tokenHandler = new JwtSecurityTokenHandler();

            return new AuthenticationResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                expirationAccessToken = expiration,
                RefreshToken = refreshToken,
                expirationRefreshToken = expiration_refreshToken
            };
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration) =>
            new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private Claim[] CreateClaims(IdentityUser user) =>
            new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

        private SigningCredentials CreateSigningCredentials() =>
            new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
    }
}
