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
  
            private readonly RedeSocialContext  _context;
            private readonly JwtBearerTokenSettings _jwtTokenSettings;
            private readonly UserManager<IdentityUser> _userManager;

            public UsersController(RedeSocialContext context, IOptions<JwtBearerTokenSettings> jwtTokenOptions, UserManager<IdentityUser> userManager)
            {
                _context = context;
                _jwtTokenSettings = jwtTokenOptions.Value;
                _userManager = userManager;
            }


            [HttpPost]
            [Route("api/Usuarios/Register")]
            public async Task<IActionResult> RegisterUser([FromBody] Usuarios usuarios)
            {
                if (!ModelState.IsValid || usuarios == null)
                {
                    return new BadRequestObjectResult(new { Message = "Falha ao realizar o registro" });
                }


                var identityUser = new IdentityUser() { UserName = usuarios.Nome, Email = usuarios.Email };

                var result = await _userManager.CreateAsync(identityUser, usuarios.Password);

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
            //[AllowAnonymous]
            [Route("Login")]
            public async Task<IActionResult> LoginUser([FromBody] Usuarios usuarios)
            {
                IdentityUser identityUser;

                if (!ModelState.IsValid ||
                    usuarios == null ||
                    (identityUser = await ValidateUser(usuarios)) == null)
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

            private async Task<IdentityUser> ValidateUser(Usuarios usuarios)
            {
                var identityUser = await _userManager.FindByEmailAsync(usuarios.Email);


                if (identityUser != null)
                {
                    var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, usuarios.Password);

                    return result == PasswordVerificationResult.Failed ? null : identityUser;
                }

                return null;
            }


        }
    }


