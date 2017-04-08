using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Server.Responses;
using SocialSurvey.Server.DTO;
using Microsoft.AspNetCore.Http.Extensions;
using SocialSurvey.Domain.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class OptionsController : Controller
    {
        IUnitOfWork _uow;

        public OptionsController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var options = _uow.Options.GetAll();
            var response = new ListResponse<OptionDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Method = Request.Method;
            response.Method = "This method is under development";
            response.Response = options.Select(o => new OptionDTO
            {
                OptionId = o.OptionId,
                Text = o.Text,
                Order = o.Order,
                IsDeleted = o.IsDeleted,
                QuestionId = o.QuestionId
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var option = _uow.Options.Get(id);
            if (option == null)
                return NotFound($"Option with id {id} is not exist");

            var response = new SingleResponse<OptionDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Message = Request.Method;
            response.Message = "This method is under development";

            response.Response = new OptionDTO
            {
                OptionId = option.OptionId,
                Text = option.Text,
                Order = option.Order,
                IsDeleted = option.IsDeleted,
                QuestionId = option.QuestionId
            };

            return Ok(response);
        }
        
        
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]OptionDTO received)
        {
            if (_uow.Options.Get(id) == null)
                return NotFound($"Option with id {id} is not exist");

            Option optionToUpdate = new Option
            {
                OptionId = id,
                Text = received.Text,
                Order = received.Order,
                IsDeleted = received.IsDeleted,
                QuestionId = received.QuestionId
            };

            _uow.Options.Update(optionToUpdate);
            _uow.Save();
            return Ok("Updated");
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_uow.Options.Get(id) == null)
                return NotFound("Option is not found");

            _uow.Options.Delete(id, true);
            _uow.Save();

            return Ok("Deleted successfully");
        }
        [HttpDelete("{id}/soft")]
        public IActionResult SoftDelete(int id)
        {
            if (_uow.Options.Get(id) == null)
                return NotFound("Option is not found");

            _uow.Options.Delete(id, false);
            _uow.Save();

            return Ok("Deleted successfully");
        }
        [HttpPatch("{id}/restore")]
        public IActionResult Restore(int id)
        {
            if (_uow.Options.Get(id) == null)
                return NotFound("Option is not found");

            _uow.Options.Restore(id);
            _uow.Save();

            return Ok("Deleted successfully");
        }
    }
}
