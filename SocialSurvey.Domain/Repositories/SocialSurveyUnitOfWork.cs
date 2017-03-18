using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Interfaces;
using SocialSurvey.Domain.DB;
using Microsoft.EntityFrameworkCore;
using SocialSurvey.Domain.Entities;

namespace SocialSurvey.Domain.Repositories
{
    public class SocialSurveyUOW : IUnitOfWork
    {
        private SocialSurveyContext _ctx;

        private UserRepository userRepository;
        private SurveyRepository surveyRepository;
        private QuestionRepository questionRepository;
        private OptionRepository optionRepository;

        public SocialSurveyUOW(DbContextOptions<SocialSurveyContext> options)
        {
            _ctx = new SocialSurveyContext(options);
        }

        public IRepository<User> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(_ctx);
                return userRepository;
            }
        }

        public IRepository<Survey> Surveys
        {
            get
            {
                if (surveyRepository == null)
                    surveyRepository = new SurveyRepository(_ctx);
                return surveyRepository;
            }
        }

        public IRepository<Question> Questions
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionRepository(_ctx);
                return questionRepository;
            }
        }

        public IRepository<Option> Options
        {
            get
            {
                if (optionRepository == null)
                    optionRepository = new OptionRepository(_ctx);
                return optionRepository;
            }
        }
        
        /// <summary>
        /// Save all changing in db
        /// </summary>
        public void Save()
        {
            _ctx.SaveChanges();
        }


        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
