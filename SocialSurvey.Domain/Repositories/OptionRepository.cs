using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;

namespace SocialSurvey.Domain.Repositories
{
    public class OptionRepository : IRepository<Option>
    {
        SocialSurveyContext _ctx;

        public OptionRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }

        public void Create(Option entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Option entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Option> Find(Func<Option, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Option Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Option> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Option entity)
        {
            throw new NotImplementedException();
        }
    }
}
