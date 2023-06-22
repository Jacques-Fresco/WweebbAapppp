using System.Security.Cryptography;
using WweebbAapppp.Models;

namespace WweebbAapppp.Services
{
    public class RefreshTokenService
    {
        private readonly VehicleQuotesContext _context;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(VehicleQuotesContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public void SaveRefreshToken(string userId, string refreshToken, DateTime expiration)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpirationDate = expiration
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges();
        }

        public bool ValidateRefreshToken(string refreshToken, string userId)
        {
            var refreshTokenEntity = _context.RefreshTokens
                .FirstOrDefault(t => t.Token == refreshToken && t.UserId == userId && t.ExpirationDate >= DateTime.UtcNow);

            return refreshTokenEntity != null;
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            var refreshTokenEntity = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);

            if (refreshTokenEntity != null)
            {
                _context.RefreshTokens.Remove(refreshTokenEntity);
                _context.SaveChanges();
            }
        }
    }
}
