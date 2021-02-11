using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TasksAPI.Models;
using TasksAPI.Repositories.Interfaces;

namespace TasksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public UserController(IApplicationUserRepository userRepository,
                             SignInManager<ApplicationUser> signInManager,
                             UserManager<ApplicationUser> userManager,
                             ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            //_signInManager = signInManager;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody]UserDTO user)
        {
            ModelState.Remove("PasswordConfirmation");
            ModelState.Remove("Name");

            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = _userRepository.Get(user.Email, user.Password);

                if (applicationUser != null)
                {
                    // Migrado a utilização de cookie para usar jwt
                    //_signInManager.SignInAsync(applicationUser, false);                    

                    return Ok(GenerateToken(applicationUser));
                }
                else
                {
                    return NotFound("Usuário não localizado!");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        [HttpPost("renew")]
        public ActionResult Renew([FromBody]TokenDTO tokenDTO)
        {
            var refreshTonken = _tokenRepository.Get(tokenDTO.RefreshToken);

            if (refreshTonken == null)
            {
                return NotFound();
            }

            refreshTonken.DateModifield = DateTime.Now;
            refreshTonken.Used = true;

            _tokenRepository.Update(refreshTonken);

            var applicationUser = _userRepository.Get(refreshTonken.UserId);

            return GenerateToken(applicationUser);

        }

        [HttpPost("")]
        public ActionResult Add([FromBody]UserDTO user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    FullName = user.Name,
                    Email = user.Email,
                    UserName = user.Email
                };

                var result = _userManager.CreateAsync(applicationUser, user.Password).Result;

                if (!result.Succeeded)
                {
                    List<string> errors = new List<string>();

                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }

                    return UnprocessableEntity(errors);
                }
                else
                {
                    return Ok(applicationUser);
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }

        public TokenDTO BuildToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key-api-jwt-tasks"));
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(issuer: null,
                                                          audience: null,
                                                          claims: claims,
                                                          expires: exp,
                                                          signingCredentials: sign);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var expRefreshToken = DateTime.UtcNow.AddHours(2);

            var tokenDTO = new TokenDTO
            {
                Token = tokenString,
                Expiration = exp,
                ExpirationRefreshToken = expRefreshToken,
                RefreshToken = refreshToken
            };

            return tokenDTO;
        }

        private ActionResult GenerateToken(ApplicationUser applicationUser)
        {
            var token = BuildToken(applicationUser);

            var newToken = new Token()
            {
                RefreshToken = token.RefreshToken,
                ExpirationToken = token.Expiration,
                ExpirationRefreshToken = token.ExpirationRefreshToken,
                User = applicationUser,
                DateCreated = DateTime.Now,
                Used = false
            };

            _tokenRepository.Add(newToken);

            return Ok(token);
        }

    }
}