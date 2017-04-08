using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Server.DTO;
using SocialSurvey.Server.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using SocialSurvey.Domain.Entities;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class QuestionsController : Controller
    {
        IUnitOfWork _uow;
        public QuestionsController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = new ListResponse<QuestionDTO>();

            response.Link = Request.GetDisplayUrl();
            response.Message = "This method is under development";
            var questions = _uow.Questions.GetAll();
            IEnumerable<QuestionDTO> responseList = questions.Select(q => new QuestionDTO
            {
                QuestionId = q.QuestionId,
                Text = q.Text,
                Order = q.Order,
                QuestionType = q.QuestionType,
                IsDeleted = q.IsDeleted,
                SurveyId = q.SurveyId
            });
            response.Response = responseList;

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = _uow.Questions.Get(id);

            if (question == null)
                return NotFound($"Question with id {id} is not exist");

            var response = new SingleResponse<QuestionDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Method = Request.Method;
            response.Message = "This method is under development";

            response.Response = new QuestionDTO
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                Order = question.Order,
                QuestionType = question.QuestionType,
                IsDeleted = question.IsDeleted,
                SurveyId = question.SurveyId,
                Options = question.Options.Select(option =>
                new OptionDTO
                {
                    Text = option.Text,
                    OptionId = option.OptionId,
                    Order = option.Order,
                    IsDeleted = option.IsDeleted
                }).ToList()
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Question received)
        {
            if (_uow.Questions.Get(id) == null)
                return NotFound($"Question with id {id} is not exist");

            List<Option> optionsToUpdate = new List<Option>();

            foreach(var option in received.Options)
            {
                optionsToUpdate.Add(new Option
                {
                    QuestionId = option.QuestionId,
                    OptionId = option.OptionId,
                    Text = option.Text,
                    Order = option.Order,
                    IsDeleted = option.IsDeleted 
                });
            }

            Question questionToUpdate = new Question
            {
                QuestionId = id,
                Text = received.Text,
                Order = received.Order,
                QuestionType = received.QuestionType,
                IsDeleted = received.IsDeleted,
                SurveyId = received.SurveyId,
                Options = optionsToUpdate
            };

            _uow.Questions.Update(questionToUpdate);
            _uow.Save();
            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_uow.Questions.Get(id) == null)
                return NotFound("Question is not found");

            _uow.Questions.Delete(id, true);
            _uow.Save();

            return Ok("Deleted successfully");
        }

        [HttpDelete("{id}/soft")]
        public IActionResult SoftDelete(int id)
        {
            if (_uow.Questions.Get(id) == null)
                return NotFound("Question is not found");

            _uow.Questions.Delete(id, false);
            _uow.Save();

            return Ok("Deleted successfully");
        }
        [HttpPatch("{id}/restore")]
        public IActionResult Restore(int id)
        {
            if (_uow.Questions.Get(id) == null)
                return NotFound("Question is not found");

            _uow.Questions.Restore(id);
            _uow.Save();

            return Ok("Restored successfully");
        }

        [HttpGet("{id}/options")]
        public IActionResult GetOptions(int id)
        {
            var question = _uow.Questions.Get(id);

            if (question == null)
                return NotFound($"Question with id {id} is not exist");

            var response = new SingleResponse<QuestionDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Method = Request.Method;
            response.Message = "This method is under development";

            response.Response = new QuestionDTO
            {
                QuestionId = question.QuestionId,
                Text = question.Text,
                Order = question.Order,
                QuestionType = question.QuestionType,
                IsDeleted = question.IsDeleted,
                SurveyId = question.SurveyId,
                Options = question.Options.Select(option =>
                new OptionDTO
                {
                    Text = option.Text,
                    OptionId = option.OptionId,
                    Order = option.Order,
                    IsDeleted = option.IsDeleted
                }).ToList()
            };

            return Ok(response);
        }
    }
}
