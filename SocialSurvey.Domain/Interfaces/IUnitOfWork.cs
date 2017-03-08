using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Entities;

namespace SocialSurvey.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Survey> Surveys { get; }
        IRepository<Question> Questions { get; }
        IRepository<Option> Options { get; }
        void Save(); 
    }
}
