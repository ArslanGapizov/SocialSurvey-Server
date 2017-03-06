using SocialSurvey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.DB
{
    public interface ISocialSurveyRepository : IDisposable
    {
        IQueryable<Survey> GetSurveys();
    }
}
