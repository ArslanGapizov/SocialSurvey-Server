using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace SocialSurvey.Server.Controllers
{
    [Route("api/[controller]")]
    public class FormsController : Controller
    {
        [HttpGet]
        public IAsyncResult Get()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet("{id}")]
        public IAsyncResult Get(int id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public IAsyncResult Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut("{id}")]
        public IAsyncResult Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{id}")]
        public IAsyncResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
