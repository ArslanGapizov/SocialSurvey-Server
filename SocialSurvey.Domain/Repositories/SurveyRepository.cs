using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.DB;
using Microsoft.EntityFrameworkCore;

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
            _ctx.Add(entity);
        }

        public void Delete(Survey entity)
        {
            entity.IsDeleted = true;
            _ctx.Entry(entity).State = EntityState.Deleted;
        }

        public IEnumerable<Survey> Find(Func<Survey, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Survey Get(int id)
        {
            return _ctx.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .SingleOrDefault(s => s.SurveyId == id);
        }

        public IEnumerable<Survey> GetAll()
        {
            return _ctx.Surveys;
        }

        public void Restore(Survey entity)
        {
            entity.IsDeleted = false;
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Survey entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}
