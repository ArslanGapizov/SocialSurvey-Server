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
        [HttpGet]
        public IActionResult Get()
        {
            var surveys = _uow.Surveys;

            var response = new ListResponse<SurveyDTO>();

            response.Link = ControllerContext.HttpContext.Request.PathBase;
            response.Message = "This method is under development";
            response.Response = new List<SurveyDTO>
            {
                new SurveyDTO { SurveyId = 1, Name = "Test1", Comment = "For testing", UserId = 1 },
                new SurveyDTO { SurveyId = 2, Name = "Test2", Comment = "For testing", UserId = 1 },
                new SurveyDTO { SurveyId = 3, Name = "Test3", Comment = "For testing", UserId = 2 },
                new SurveyDTO { SurveyId = 4, Name = "Test4", Comment = "For testing", UserId = 1 },
                new SurveyDTO { SurveyId = 5, Name = "Test5", Comment = "For testing", UserId = 3 },
            };
            return Ok(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var survey = _uow.Surveys.Get(id);

            //var response = new SingleResponse<Survey>();
            var response = new SingleResponse<SurveyDTO>();
            response.Link = ControllerContext.HttpContext.Request.PathBase;
            response.Message = "This method is under development";
            //response.Response = new SurveyDTO
            //{
            //    SurveyId = 2,
            //    Name = "Test2",
            //    Comment = "For testing",
            //    UserId = 3,
            //    Questions = new List<QuestionDTO>()
            //};
            //for (int i = 0; i < 20; i++)
            //{
            //    var question = new QuestionDTO
            //    {
            //        Text = "Question" + i,
            //        QuestionId = i + 56,
            //        Order = i,
            //        QuestionType = QuestionType.Select,
            //        Options = new List<OptionDTO>()
            //    };
            //    for (int q = 0; q < 4; q++)
            //    {

            //        question.Options.Add(new OptionDTO
            //        {
            //            Text = question.Text + "option" + i,
            //            OptionId = (i+56)*(q+1),
            //            Order = q
            //        });
            //    }
            //    response.Response.Questions.Add(question);

            //}
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
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
