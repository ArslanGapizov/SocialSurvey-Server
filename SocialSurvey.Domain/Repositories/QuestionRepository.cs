using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;

namespace SocialSurvey.Domain.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        SocialSurveyContext _ctx;

        public QuestionRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }

        public void Create(Question entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Question entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> Find(Func<Question, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Question Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Question> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Question entity)
        {
            throw new NotImplementedException();
        }
    }
}
