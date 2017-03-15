using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Server.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Server.DTO;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Server.Responses;
using Microsoft.AspNetCore.Authorization;

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        IUnitOfWork _uow;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            User currentUser = _uow.Users.Find(x => x.Login == User.Identity.Name).FirstOrDefault();

            SingleResponse<UserDTO> response = new SingleResponse<UserDTO>();
            response.Link = Request.Path;
            response.Message = "GET";
            response.Message = "Method under development";
            response.Response = new UserDTO
            {
                UserId = currentUser.UserId,
                Login = currentUser.Login,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                MiddleName = currentUser.MiddleName,
            };

            return Ok(response);
        }
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] UserDTO user)
        {
            User currentUser = _uow.Users.Find(x => x.Login == User.Identity.Name).FirstOrDefault();

            if (!string.IsNullOrEmpty(user.FirstName))
                currentUser.FirstName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
                currentUser.LastName = user.LastName;
            if (!string.IsNullOrEmpty(user.MiddleName))
                currentUser.MiddleName = user.MiddleName;

            _uow.Users.Update(currentUser);
            _uow.Save();
            
            return Ok("Succeeded");
        }
        [Authorize]
        [HttpPut("/api/account/password")]
        public IActionResult Put()
        {
            var oldPassword = Request.Form["oldPassword"];
            var password = Request.Form["password"];
            var passwordConfirm = Request.Form["passwordConfirm"];

            if (password != passwordConfirm)
                return BadRequest("Passwords do not match");

            var hashedOld = HashConverter.GetHash(oldPassword);


            User currentUser = _uow.Users.
                Find(x => x.Login == User.Identity.Name && 
                          x.PasswordHash == hashedOld)
                .FirstOrDefault();

            if (currentUser == null)
                return BadRequest("Wrong password");

            currentUser.PasswordHash = HashConverter.GetHash(password);
            _uow.Users.Update(currentUser);
            _uow.Save();

            return Ok("Succeeded");
        }
        /// <summary>
        /// Action for getting token for request form`s login and password
        /// </summary>
        [HttpPost("/token")]
        public IActionResult Token()
        {
            var login = Request.Form["login"];
            var password = Request.Form["password"];

            var hashedPassword = HashConverter.GetHash(password);
            var identity = GetIdentity(login, hashedPassword);

            if (identity == null)
            {
                return BadRequest("Invalid login or password.");
            }

            var response = GenerateToken(identity);

            return Ok(response);
        }
        
        /// <summary>
        /// Action for registation user, returns access token
        /// </summary>
        [HttpPost("/register")]
        public IActionResult Register([FromBody] RegisterDTO registerData)
        {
            if (registerData == null)
                return BadRequest("Data is null");
            if (registerData.Password != registerData.PasswordConfirm)
                return BadRequest("Passwords do not match");


            var hashedPassword = HashConverter.GetHash(registerData.Password);
            var result = CreateUser(login: registerData.Login,
                                    password: hashedPassword,
                                    firstName: registerData.FirstName,
                                    lastName: registerData.LastName,
                                    middleName: registerData.MiddleName);

            if (result)
            {
                var identity = GetIdentity(registerData.Login,
                                           hashedPassword);
                var TokenResponse = GenerateToken(identity);
                return Ok(TokenResponse);
            }
            return BadRequest("Cannot create user");
        }
        private bool CreateUser(string login,
                                string password,
                                string firstName,
                                string lastName,
                                string middleName)
        {
            if (login == null ||
               password == null)
                return false;

            try
            {
                _uow.Users.Create(new User
                {
                    Login = login,
                    PasswordHash = password,
                    FirstName = firstName,
                    LastName = lastName,
                    MiddleName = middleName,
                    Role = Role.Interviewer,
                    CreationDate = new DateTime()
                });
                _uow.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method for generation token from identity
        /// </summary>
        private TokenDTO GenerateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenDTO
            {
                AccessToken = encodedJwt,
                Login = identity.Name,
                ExpiresIn = (jwt.ValidTo - jwt.ValidFrom).TotalSeconds
            };
        }

        /// <summary>
        /// Gets user from db by login and password
        /// </summary>
        private ClaimsIdentity GetIdentity(string login, string password)
        {
            User user = _uow.Users.Find(x => x.Login == login && x.PasswordHash == password).FirstOrDefault();

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

    }
}
