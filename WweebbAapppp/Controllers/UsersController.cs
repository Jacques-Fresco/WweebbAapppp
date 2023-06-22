using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VehicleQuotes.ResourceModels;
using System.Threading.Tasks;
using WweebbAapppp.Services;
using WweebbAapppp.ResourceModels;
using WweebbAapppp.Models;
using System.Runtime.InteropServices;
using WweebbAapppp.Controllers;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using WweebbAapppp;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace VehicleQuotes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtService _jwtService;
        private readonly VehicleQuotesContext _context;
        private readonly RefreshTokenService _refreshTokenService;

        public UsersController(UserManager<IdentityUser> userManager, JwtService jwtService, VehicleQuotesContext context, RefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _context = context;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user_exist = await _userManager.FindByEmailAsync(user.Email);

            if (user_exist != null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>
                    {
                        "User already exist"
                    }
                });
            }

            var new_user = new IdentityUser()
            {
                Email = user.Email,
                UserName = user.UserName,
                EmailConfirmed = false
            };


            var is_created = await _userManager.CreateAsync(new_user, user.Password);

            if (!is_created.Succeeded)
            {
                return BadRequest(is_created.Errors);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(new_user);

            await Task.Delay(10000);


            await _userManager.SetAuthenticationTokenAsync(new_user, "Default", "EmailConfirmation", code);

            var email_body = $"Please confirm your email address <a href=\"#URL#\">Click me<a/>";

            var callback_url = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Users",
                new { userId = new_user.Id, code = code });

            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callback_url));

            // SEND EMAIL
            var result = SendEmail(body);

            return Ok(result ? "Please verify your email, through the verification email we have just sent." : "Please request an email verification link");


            /*var registeredUser = await _userManager.FindByEmailAsync(user.Email);

            var authenticationResponse = _jwtService.CreateToken(registeredUser);*/

            //user.Password = null;
            //return CreatedAtAction("GetUser", new { username = user.UserName }, user);
            //return Created("", user);

            //return Ok(authenticationResponse);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return new User
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
            else if (user.EmailConfirmed == false)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Email needs to be confirmed"
                    }
                });
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var token = _jwtService.CreateToken(user);
            return Ok(token);
        }

        [Route("ConfirmEmail")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null && code == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid email confirmation url"
                    }
                });
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid email parameter"
                    }
                });
            }

            // code = Encoding.UTF8.GetString(Convert.FromBase64String(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded) { 
                var authenticationResponse = _jwtService.CreateToken(user);
                return Ok(authenticationResponse);
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                    {
                        "Your email is not confirmed, please try again later"
                    }
            });
        }

        [HttpGet("resend-confirmation-email")]
        public async Task<ActionResult> ResendConfirmationEmail(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            if (user.EmailConfirmed)
            {
                return BadRequest("Email is already confirmed.");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.SetAuthenticationTokenAsync(user, "Default", "EmailConfirmation", code);

            var email_body = "Please confirm your email address <a href=\"#URL#\">Click me<a/>";
            var callback_url = Request.Scheme + "://" + Request.Host + Url.Action("ConfirmEmail", "Users",
                new { userId = user.Id, code = code });
            var body = email_body.Replace("#URL#", System.Text.Encodings.Web.HtmlEncoder.Default.Encode(callback_url));

            // Отправка подтверждающего сообщения
            var result = SendEmail(body);

            if (result)
            {
                return Ok("Confirmation email has been resent.");
            }
            else
            {
                return BadRequest("Failed to resend confirmation email.");
            }
        }

        [HttpPost("generate-new-token")]
        public async Task<IActionResult> GenerateAccessToken(TokensRequest tokens)
        {
            if (!ModelState.IsValid)
            {
                // Обработка невалидных данных
                return BadRequest(ModelState);
            }

            /*string accessToken = tokens.AccessToken;
            string payload = accessToken.Split('.')[1];
            byte[] payloadBytes = Convert.FromBase64String(payload);
            string decodedPayload = Encoding.UTF8.GetString(payloadBytes);
            JObject payloadJson = JObject.Parse(decodedPayload);
            string userId = (string)payloadJson[ClaimTypes.NameIdentifier];*/

            string accessToken = tokens.AccessToken;
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(accessToken);
            var payload = jwtToken.Payload.SerializeToJson();
            JObject payloadJson = JObject.Parse(payload);
            string userId = (string)payloadJson[ClaimTypes.NameIdentifier];

            // Validate the refresh token
            var isValidRefreshToken = _refreshTokenService.ValidateRefreshToken(tokens.RefreshToken, userId);
            if (!isValidRefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            // Get the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Create new access token and refresh token
            var newAccessToken = _jwtService.CreateToken(user);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            // Save the new refresh token
            var expirationRefreshToken = DateTime.UtcNow.AddMinutes(JwtService.EXPIRATION_MINUTES_REFRESH_TOKEN);
            _refreshTokenService.SaveRefreshToken(user.Id, newRefreshToken, expirationRefreshToken);

            // Return the new tokens
            var response = new AuthenticationResponse
            {
                AccessToken = newAccessToken.AccessToken,
                expirationAccessToken = newAccessToken.expirationAccessToken,
                RefreshToken = newRefreshToken,
                expirationRefreshToken = expirationRefreshToken
            };

            return Ok(response);
        }

        private bool SendEmail(string body, string _email = "")
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("montana.lind38@ethereal.email"));
                email.To.Add(MailboxAddress.Parse("montana.lind38@ethereal.email"));
                email.Subject = "Test Email Subject";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("monique85@ethereal.email", "7SpkeyZ7zTXYz513Md");
                smtp.Send(email);
                smtp.Disconnect(true);

                return true; // отправка прошла успешно
            }
            catch (Exception ex)
            {
                // Обработка возможных исключений при отправке письма
                Console.WriteLine($"Ошибка отправки письма: {ex.Message}");
                return false; // отправка не удалась
            }
        }
    }
}