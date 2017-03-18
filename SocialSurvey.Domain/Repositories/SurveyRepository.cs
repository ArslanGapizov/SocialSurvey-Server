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

        public void Delete(int id, bool hard = true)
        {
            Survey surveyToDelete = Get(id);
            if (surveyToDelete == null)
                throw new ArgumentOutOfRangeException($"There is no survey with id - {id}");

            surveyToDelete.IsDeleted = true;
            foreach (var question in surveyToDelete.Questions)
            {
                question.IsDeleted = true;
                foreach (var option in question.Options)
                    option.IsDeleted = true;
            }
            _ctx.Entry(surveyToDelete).State = EntityState.Modified;
        }

        public void Delete(Survey entity, bool hard = true)
        {
            entity.IsDeleted = true;
            _ctx.Entry(entity).State = EntityState.Deleted;
        }

        public IEnumerable<Survey> Find(Func<Survey, bool> predicate)
        {
            return _ctx.Surveys.Where(predicate);
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

        public void Restore(int id)
        {
            Survey surveyToDelete = Get(id);
            if (surveyToDelete == null)
                throw new ArgumentOutOfRangeException($"There is no survey with id - {id}");

            surveyToDelete.IsDeleted = false;
            foreach (var question in surveyToDelete.Questions)
            {
                question.IsDeleted = false;
                foreach (var option in question.Options)
                    option.IsDeleted = false;
            }
            _ctx.Entry(surveyToDelete).State = EntityState.Modified;
        }

        public void Restore(Survey entity)
        {
            entity.IsDeleted = false;
            if (entity.Questions != null)
                foreach (var question in entity.Questions)
                {
                    question.IsDeleted = false;
                    if (question.Options != null)
                        foreach (var option in question.Options)
                            option.IsDeleted = false;
                }

            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Survey entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}
