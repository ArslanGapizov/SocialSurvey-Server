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
    public class QuestionRepository : IRepository<Question>
    {
        SocialSurveyContext _ctx;

        public QuestionRepository(SocialSurveyContext context)
        {
            _ctx = context;
        }

        public void Create(Question entity)
        {
            _ctx.Questions.Add(entity);
        }

        public void Delete(int id)
        {
            Question questionToDelete = Get(id);
            if (questionToDelete == null)
                throw new ArgumentOutOfRangeException($"There are no question with id - {id}");

            questionToDelete.IsDeleted = true;
            foreach (var option in questionToDelete.Options)
                option.IsDeleted = true;

            _ctx.Entry(questionToDelete).State = EntityState.Modified;
        }

        public void Delete(Question entity)
        {
            entity.IsDeleted = true;
            if (entity.Options != null)
                foreach (var option in entity.Options)
                    option.IsDeleted = true;

            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<Question> Find(Func<Question, bool> predicate)
        {
            return _ctx.Questions.Where(predicate);
        }

        public Question Get(int id)
        {
            return _ctx.Questions
                .Include(q => q.Options)
                .SingleOrDefault(s => s.SurveyId == id);
        }

        public IEnumerable<Question> GetAll()
        {
            return _ctx.Questions;
        }

        public void Restore(int id)
        {
            Question questionToRestore = Get(id);
            if (questionToRestore == null)
                throw new ArgumentOutOfRangeException($"There are no question with id - {id}");

            questionToRestore.IsDeleted = false;
            foreach (var option in questionToRestore.Options)
                option.IsDeleted = false;
            _ctx.Entry(questionToRestore).State = EntityState.Modified;
        }

        public void Restore(Question entity)
        {
            entity.IsDeleted = false;

            if (entity.Options != null)
                foreach (var option in entity.Options)
                    option.IsDeleted = false;

            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Update(Question entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }
    }
}
