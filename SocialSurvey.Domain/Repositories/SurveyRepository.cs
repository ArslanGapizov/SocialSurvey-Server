using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;

namespace SocialSurvey.Domain.Repositories
{
    public class SurveyRepository : IRepository<Survey>
    {
        private SocialSurveyContext _ctx;

        public SurveyRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }
        public void Create(Survey entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Survey entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Survey> Find(Func<Survey, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Survey Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Survey> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Survey entity)
        {
            throw new NotImplementedException();
        }
    }
}
