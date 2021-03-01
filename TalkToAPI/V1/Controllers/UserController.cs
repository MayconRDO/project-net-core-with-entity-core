using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TalkToAPI.V1.Models;
using TalkToAPI.V1.Models.DTO;
using TalkToAPI.V1.Repositories.Contracts;

namespace TalkToAPI.V1.Controllers
{
    /// <summary>
    /// Controle o objeto usuário
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="userRepository">Repositório do Usuário</param>
        /// <param name="signInManager">Login da classe UseManager</param>
        /// <param name="userManager">Interface dos serviços da classe UserManager</param>
        /// <param name="tokenRepository">Repositório do Token</param>
        public UserController(IApplicationUserRepository userRepository,
                             UserManager<ApplicationUser> userManager,
                             ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;
            //_signInManager = signInManager;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Realizar login
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserDTO user)
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

        /// <summary>
        /// Obter todos os usuários
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("")]
        public ActionResult GetAll()
        {
            return Ok(_userManager.Users);
        }

        /// <summary>
        /// Obter todos os usuário pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Ronovar token
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <returns></returns>
        [HttpPost("renew")]
        public ActionResult Renew([FromBody] TokenDTO tokenDTO)
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

        /// <summary>
        /// Cadastrar usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("")]
        public ActionResult Add([FromBody] UserDTO user)
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

        /// <summary>
        /// Alterar usuário
        /// </summary>
        /// <param name="id">ID identificador do usuário</param>
        /// <param name="user">DTO usuário</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Update(string id, [FromBody] UserDTO user)
        {
            ApplicationUser applicationUser = _userManager.GetUserAsync(HttpContext.User).Result;

            if (ModelState.IsValid)
            {
                applicationUser.FullName = user.Name;
                applicationUser.Email = user.Email;
                applicationUser.UserName = user.Email;
                applicationUser.Slogan = user.Slogan;

                var result = _userManager.UpdateAsync(applicationUser).Result;
                _userManager.RemovePasswordAsync(applicationUser);
                _userManager.AddPasswordAsync(applicationUser, user.Password);

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

        #region Métodos privados

        private TokenDTO BuildToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key-api-jwt-talk-to"));
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

        #endregion

    }
}
