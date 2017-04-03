using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Server.DTO;
using SocialSurvey.Server.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;
using SocialSurvey.Server.Auth;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        IUnitOfWork _uow;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<User> users = _uow.Users.GetAll();

            IEnumerable<UserDTO> usersResponse = users.Select(u =>
                new UserDTO
                {
                    UserId = u.UserId,
                    Login = u.Login,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    MiddleName = u.MiddleName,
                    Role = u.Role,
                    IsDeleted = u.IsDeleted
                }
            );

            ListResponse<UserDTO> response = new ListResponse<UserDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Message = "This method is under development";
            response.Method = "GET";
            response.Response = usersResponse;

            return Ok(response);
        }


        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            User user = _uow.Users.Get(id);
            if (user == null)
                return BadRequest("User is not found");

            UserDTO userResponse = new UserDTO
            {
                UserId = user.UserId,
                Login = user.Login,
                IsDeleted = user.IsDeleted,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Role = user.Role
            };

            SingleResponse<UserDTO> response = new SingleResponse<UserDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Message = "This method is under development";
            response.Method = "GET";
            response.Response = userResponse;
            
            return Ok(response);
        }

        [HttpGet("{login}")]
        public IActionResult GetByLogin(string login)
        {
            User user = _uow.Users.Find(x => x.Login == login).SingleOrDefault();
            if (user == null)
                return BadRequest("User is not found");

            UserDTO userResponse = new UserDTO
            {
                UserId = user.UserId,
                Login = user.Login,
                IsDeleted = user.IsDeleted,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Role = user.Role
            };

            SingleResponse<UserDTO> response = new SingleResponse<UserDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Message = "This method is under development";
            response.Method = "GET";
            response.Response = userResponse;

            return Ok(response);
        }


        //TODO
        [HttpGet("{id}/Surveys")]
        public IActionResult GetUserSurveys(int id)
        {

            IEnumerable<Survey> surveys = _uow.Surveys.Find(x => x.UserId == id);
            

            ListResponse<SurveyDTO> response = new ListResponse<SurveyDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Message = $"User`s surveys with {id}";
            response.Method = "GET";

            IEnumerable<SurveyDTO> responseList = surveys.Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                Name = s.Name,
                Comment = s.Comment,
                IsDeleted = s.IsDeleted,
                UserId = s.UserId
            });
            response.Response = responseList;

            return Ok(response);
        }

        // TODO
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }
        
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]UserDTO data)
        {
            if (_uow.Users.Get(id) == null)
                return BadRequest($"User with id - {id} is not exist");

            User userToUpdate = new User()
            {
                UserId = id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                MiddleName = data.MiddleName,
                IsDeleted = data.IsDeleted,
                Role = data.Role,
                PasswordHash = HashConverter.GetHash(data.Password)
            };

            _uow.Users.Update(userToUpdate);
            _uow.Save();

            return Ok("Updated");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_uow.Users.Get(id) == null)
                return BadRequest("User is not found");

            _uow.Users.Delete(id, true);
            _uow.Save();

            return Ok("Deleted successfully");
        }

        [HttpDelete("{id}/soft")]
        public IActionResult SoftDelete(int id)
        {
            if (_uow.Users.Get(id) == null)
                return BadRequest("User is not found");

            _uow.Users.Delete(id, false);
            _uow.Save();

            return Ok("Deleted successfully");
        }
        [HttpPatch("{id}/restore")]
        public IActionResult Restore(int id)
        {
            if (_uow.Users.Get(id) == null)
                return NotFound("Survey is not found");

            _uow.Users.Restore(id);
            _uow.Save();

            return Ok("Restored successfully");
        }
    }
}
