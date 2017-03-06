using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Entities;

namespace SocialSurvey.Domain.DB
{
    public class SocialSurveyRepository : ISocialSurveyRepository
    {
        private readonly SocialSurveyContext _dbContext;

        public SocialSurveyRepository(SocialSurveyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        public IQueryable<Survey> GetSurveys()
        {
            var query = _dbContext.Surveys;

            return query;
        }
    }
}
