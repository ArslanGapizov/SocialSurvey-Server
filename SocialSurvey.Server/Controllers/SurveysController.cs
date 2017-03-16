using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Server.DTO;
using SocialSurvey.Server.Responses;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class SurveysController : Controller
    {
        IUnitOfWork _uow;
        public SurveysController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET api/surveys
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var response = new ListResponse<SurveyDTO>();

            response.Link = Request.GetDisplayUrl();
            response.Message = "This method is under development";
            var surveys = _uow.Surveys.GetAll();
            IEnumerable<SurveyDTO> responseList = surveys.Select(s => new SurveyDTO
            {
                SurveyId = s.SurveyId,
                Name = s.Name,
                Comment = s.Comment,
                UserId = s.UserId
            });
            response.Response = responseList;

            return Ok(response);
        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var survey = _uow.Surveys.Get(id);

            if (survey == null)
                return NotFound($"Survey with id-{id} is not exist");

            var response = new SingleResponse<SurveyDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Method = Request.Method;
            response.Message = "This method is under development";
            
            response.Response = new SurveyDTO
            {
                SurveyId = survey.SurveyId,
                Name = survey.Name,
                Comment = survey.Comment,
                UserId = survey.UserId,
                Questions = survey
                            .Questions
                            .Select(question =>
                            new QuestionDTO
                            {
                                QuestionId = question.QuestionId,
                                Text = question.Text,
                                Order = question.Order,
                                QuestionType = question.QuestionType,
                                Options = question.Options.Select(option =>
                                    new OptionDTO
                                    {
                                        Text = option.Text,
                                        OptionId = option.OptionId,
                                        Order = option.Order
                                    }
                                ).ToList()
                            }
                ).ToList()
            };
            return Ok(response);
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] SurveyDTO survey)
        {
            User currentUser = _uow.Users.Find(x => x.Login == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return Unauthorized();

            Survey entry = new Survey()
            {
                UserId = currentUser.UserId,
                Name = survey.Name,
                Comment = survey.Comment,
                Questions = survey
                            .Questions
                            .Select(dtoQuestion => new Question
                            {
                                Text = dtoQuestion.Text,
                                QuestionType = dtoQuestion.QuestionType,
                                Order = dtoQuestion.Order,
                                Options = dtoQuestion.Options.Select(dtoOption => new Option
                                {
                                    Text = dtoOption.Text,
                                    Order = dtoOption.Order
                                }).ToList()
                            }).ToList()
            };

            _uow.Surveys.Create(entry);
            _uow.Save();

            return Ok($"Successful created, ID: {entry.SurveyId}");
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] SurveyDTO value)
        {

        }

        // DELETE api/values/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [Authorize]
        [HttpGet("{id}/questions")]
        public IActionResult GetQuestions(int id)
        {
            var survey = _uow.Surveys.Get(id);

            if (survey == null)
                return NotFound($"Survey with id-{id} is not exist");

            var response = new ListResponse<QuestionDTO>();
            response.Link = Request.GetDisplayUrl();
            response.Method = Request.Method;
            response.Message = "This method is under development";
            response.Response = survey.Questions.Select(question =>
            new QuestionDTO
            {
                Text = question.Text,
                Order = question.Order,
                QuestionType = question.QuestionType,
                QuestionId = question.QuestionId,
                SurveyId = question.SurveyId
            }).ToList();

            return Ok(response);
        }
    }
}
