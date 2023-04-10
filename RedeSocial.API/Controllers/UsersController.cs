using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RedeSocial.BLL.Configuration;
using RedeSocial.BLL.Models;
using RedeSocial.DOMAIN;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RedeSocial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {

        private readonly RedeSocialContext _context;
        private readonly JwtBearerTokenSettings _jwtTokenSettings;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(RedeSocialContext context, IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _jwtTokenSettings = jwtTokenOptions.Value;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("api/Users/Register")]
        public async Task<IActionResult> RegisterUser([FromBody] Users  user )
        {
            if (!ModelState.IsValid || user == null)
            {
                return new BadRequestObjectResult(new { Message = "Falha ao realizar o registro" });
            }


            var identityUser = new IdentityUser() { UserName = user.Nome, Email = user.Email };

            var result = await _userManager.CreateAsync(identityUser, user.Password);

            if (!result.Succeeded)
            {
                var dictionary = new ModelStateDictionary();

                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);

                }

                return new BadRequestObjectResult(new { Message = "Falha ao registrar o usuário", Errors = dictionary });
            }

            return Ok(new { Message = "Usuário registrado com sucesso" });

        }

        [HttpPost]
        [Route("api/Users/Login")]
        //[AllowAnonymous]
      
        public async Task<IActionResult> LoginUser([FromBody] Users user )
        {
            IdentityUser identityUser;

            if (!ModelState.IsValid ||
                user == null ||
                (identityUser = await ValidateUser(user)) == null)
            {
                return new BadRequestObjectResult(new { Message = "Falha ao logar o usuário" });
            }

            var token = GenerateToken(identityUser);

            return Ok(token);
        }

        private object GenerateToken(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtTokenSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
                    new Claim(ClaimTypes.Email, identityUser.Email)
                }),

                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtTokenSettings.Audience,
                Issuer = _jwtTokenSettings?.Issuer,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        private async Task<IdentityUser> ValidateUser(Users user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Email);


            if (identityUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, user.Password);

                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }


    }
}


