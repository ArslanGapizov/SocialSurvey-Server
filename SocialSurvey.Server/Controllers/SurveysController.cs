using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Server.DTO;
using SocialSurvey.Server.Responses;
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

        /// <summary>
        /// Action for getting all Surveys
        /// </summary>
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
                IsDeleted = s.IsDeleted,
                UserId = s.UserId
            });
            response.Response = responseList;

            return Ok(response);
        }

        /// <summary>
        /// Action for getting survey by id
        /// </summary>
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
                IsDeleted = survey.IsDeleted,
                Questions = survey
                            .Questions
                            .Select(question =>
                            new QuestionDTO
                            {
                                QuestionId = question.QuestionId,
                                Text = question.Text,
                                Order = question.Order,
                                QuestionType = question.QuestionType,
                                IsDeleted = question.IsDeleted,
                                Options = question.Options.Select(option =>
                                    new OptionDTO
                                    {
                                        Text = option.Text,
                                        OptionId = option.OptionId,
                                        Order = option.Order,
                                        IsDeleted = option.IsDeleted
                                    }
                                ).ToList()
                            }
                ).ToList()
            };
            return Ok(response);
        }

        /// <summary>
        /// Action for adding new survey
        /// </summary>
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

        /// <summary>
        /// Action for changing survey`s data by id
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SurveyDTO received)
        {
            if (_uow.Surveys.Get(id) == null)
                return NotFound($"Survey with id - {id} is not exist");

            List<Question> questionsToUpdate = new List<Question>();

            foreach (var question in received.Questions)
            {
                List<Option> optionsToUpdate = new List<Option>();

                foreach (var option in question.Options)
                {
                    optionsToUpdate.Add(new Option
                    {
                        QuestionId = question.QuestionId,
                        OptionId = option.OptionId,
                        Text = option.Text,
                        Order = option.Order,
                        IsDeleted = /*received.IsDeleted ? true
                                    : question.IsDeleted ? true
                                    :*/ option.IsDeleted,
                    });
                }

                questionsToUpdate.Add(new Question
                {
                    SurveyId = received.SurveyId,
                    QuestionId = question.QuestionId,
                    Text = question.Text,
                    QuestionType = question.QuestionType,
                    IsDeleted = /*received.IsDeleted ? true
                                :*/ question.IsDeleted,
                    Order = question.Order,
                    Options = optionsToUpdate
                });
            }

            Survey surveyToUpdate = new Survey
            {
                SurveyId = id,

                UserId = received.UserId,
                Name = received.Name,
                Comment = received.Comment,
                IsDeleted = received.IsDeleted,
                Questions = questionsToUpdate
            };

            _uow.Surveys.Update(surveyToUpdate);
            _uow.Save();
            return Ok("Updated");
        }

        /// <summary>
        /// Action for deleting survey by id
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_uow.Surveys.Get(id) == null)
                return NotFound("Survey is not found");

            _uow.Surveys.Delete(id, true);
            _uow.Save();

            return Ok("Deleted successfully");
        }

        [Authorize]
        [HttpDelete("{id}/soft")]
        public IActionResult SoftDelete(int id)
        {
            if (_uow.Surveys.Get(id) == null)
                return NotFound("Survey is not found");

            _uow.Surveys.Delete(id, false);
            _uow.Save();

            return Ok("Deleted successfully");
        }

        [Authorize]
        [HttpPatch("{id}/restore")]
        public IActionResult Restore(int id)
        {
            if (_uow.Surveys.Get(id) == null)
                return NotFound("Survey is not found");

            _uow.Surveys.Restore(id);
            _uow.Save();

            return Ok("Restored successfully");
        }
        /// <summary>
        /// Action for getting questions by survey`s id
        /// </summary>
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
                IsDeleted = question.IsDeleted,
                SurveyId = question.SurveyId
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Action for adding question to survey by its id
        /// </summary>
        [Authorize]
        [HttpPost("{id}/questions")]
        public IActionResult PostQuestion(int id, [FromBody] QuestionDTO question)
        {
            var survey = _uow.Surveys.Get(id);

            if (survey == null)
                return NotFound($"Survey with id-{id} is not exist");
            
            int maxOrder = survey.Questions.Max(x => x.Order);
            var newQuestion = new Question
            {
                Text = question.Text,
                Order = maxOrder + 1,
                QuestionType = question.QuestionType,
                Options = question.Options.Select(option =>
                                    new Option
                                    {
                                        Text = option.Text,
                                        Order = option.Order
                                    }).ToList()
            };
            survey.Questions.Add(newQuestion);

            return Ok($"Question added, id - {newQuestion.QuestionId}");
        }
    }
}