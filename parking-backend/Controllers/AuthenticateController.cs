using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using parking_backend.Configs;
using parking_backend.DataRequest;
using parking_backend.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace parking_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly JwtConfig _jwtConfig;

        public AuthenticateController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtConfig> optionMonitor)
        {
            this._userManager = userManager;
            this._singInManager = signInManager;
            this._jwtConfig = optionMonitor.Value;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> Register([FromHeader] FromHeaderRequest headers)
        {
            //AnTran@2021-06-27: Get Data from Request header and decode data from header
            var requestAuthorization = System.Convert.FromBase64String(headers.Authorization);
            string[] decodeAuthorization = Encoding.UTF8.GetString(requestAuthorization).Split(":");
            SurerUserlogin user = new SurerUserlogin();
            if (decodeAuthorization.Count() > 0)
            {
                user.Email = decodeAuthorization[0];
                user.Password = decodeAuthorization[1];
                user.FirstName = decodeAuthorization[2];
                user.LastName = decodeAuthorization[3];
                user.PhoneNumber = decodeAuthorization[4];
            }
            else
            {
                return BadRequest(new AuthenticateResponse()
                {
                    Errors = new List<string>()
                        {
                            "No data from request header"
                        },
                    Success = false
                });
            }
            if (ModelState.IsValid)
            {
                
                var existingUser = await this._userManager.FindByEmailAsync(user.Email);

                if(existingUser != null)
                {
                    return BadRequest(new AuthenticateResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email already existed"
                        },
                        Success = false
                    });
                }
                var newUser = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Email,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
                var isExist = await this._userManager.CreateAsync(newUser, user.Password);
                if (isExist.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);

                    return Ok(new AuthenticateResponse()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new AuthenticateResponse()
                    {
                        Errors = isExist.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }

            return BadRequest(new AuthenticateResponse()
            {
                Errors = new List<string>()
                {
                    "Invalid Client"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromHeader] FromHeaderRequest headers)
        {
            var requestAuthorization = System.Convert.FromBase64String(headers.Authorization);
            string[] decodeAuthorization = Encoding.UTF8.GetString(requestAuthorization).Split(":");
            AuthenticateRequest user = new AuthenticateRequest();
            if (decodeAuthorization.Count() > 0)
            {
                user.UserEmail = decodeAuthorization[0];
                user.Password = decodeAuthorization[1];
            }
            else
            {
                return BadRequest(new AuthenticateResponse()
                {
                    Errors = new List<string>()
                        {
                            "No data from request header"
                        },
                    Success = false
                });
            }
            if (ModelState.IsValid)
            {
                var userExist = await this._userManager.FindByEmailAsync(user.UserEmail);

                if (userExist == null)
                {
                    return BadRequest(new AuthenticateResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid Email"
                            },
                        Success = false
                    });
                }

                var isExist = await this._userManager.CheckPasswordAsync(userExist, user.Password);

                if (!isExist)
                {
                    return BadRequest(new AuthenticateResponse()
                    {
                        Errors = new List<string>() {
                                "Invalid password"
                            },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(userExist);

                return Ok(new AuthenticateResponse()
                {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new AuthenticateResponse()
            {
                Errors = new List<string>() {
                        "Invalid user account"
                    },
                Success = false
            });
        }


        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),//1h will be expired
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}
