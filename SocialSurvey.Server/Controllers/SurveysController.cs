using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class SurveysController : Controller
    {
        SocialSurveyContext _ctx;
        public SurveysController(SocialSurveyContext context)
        {
            _ctx = context;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var surveys = await _ctx.Surveys.ToArrayAsync();

            var response = surveys.Select(s => new
            {
                meta = new
                {
                    href = Request.Path.ToString()
                },
                surveyId = s.SurveyId,
                userId = s.UserId,
                name = s.Name,
                comment = s.Comment
            });

            return Ok(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
