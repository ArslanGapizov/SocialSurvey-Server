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

            response.Link = ControllerContext.HttpContext.Request.PathBase;
            response.Message = "This method is under development";
            var surveys = _uow.Surveys.GetAll();
            IEnumerable<SurveyDTO> responseList = surveys.Select(s => new SurveyDTO { SurveyId = s.SurveyId,
                                                                                      Name = s.Name,
                                                                                      Comment = s.Comment,
                                                                                      UserId = s.UserId});
            response.Response = responseList;

            return Ok(response);
        }

        // GET api/values/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var survey = _uow.Surveys.Get(id);

            //var response = new SingleResponse<Survey>();
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
                Questions = new List<QuestionDTO>()
            };
            foreach (var q in survey.Questions)
            {
                var question = new QuestionDTO
                {
                    Text = q.Text,
                    QuestionId = q.QuestionId,
                    Order = q.Order,
                    QuestionType = q.QuestionType,
                    Options = new List<OptionDTO>()
                };
                foreach(var o in q.Options)
                {
                    question.Options.Add(new OptionDTO
                    {
                        Text = o.Text,
                        Order = o.Order,
                        OptionId = o.OptionId
                    });
                }
                response.Response.Questions.Add(question);
            }
            return Ok(response);
        }

        // POST api/values
        [Authorize]
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
