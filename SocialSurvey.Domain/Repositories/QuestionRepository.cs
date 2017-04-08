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
            _ctx.Add(entity);
        }

        public void Delete(int id, bool hard = false)
        {
            var questionToDelete = _ctx.Questions.Find(id);

            if (hard)
            {
                _ctx.Questions.Remove(questionToDelete);
            }
            else
            {
                questionToDelete.IsDeleted = true;
                _ctx.Entry(questionToDelete).State = EntityState.Modified;
            }
        }

        public void Delete(Question entity, bool hard = false)
        {
            var questionToDelete = _ctx.Questions.Find(entity.QuestionId);
            if (hard)
            {
                _ctx.Questions.Remove(questionToDelete);
            }
            else
            {
                questionToDelete.IsDeleted = true;
                _ctx.Entry(questionToDelete).State = EntityState.Modified;
            }
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
            var questionToRestore = _ctx.Questions.Find(id);
            if (questionToRestore == null)
                throw new ArgumentOutOfRangeException($"There is no question with id - {id}");

            questionToRestore.IsDeleted = false;
            _ctx.Entry(questionToRestore).State = EntityState.Modified;
        }

        public void Restore(Question entity)
        {
            var questionToRestore = _ctx.Questions.Find(entity.QuestionId);
            if (questionToRestore == null)
                throw new ArgumentOutOfRangeException($"There is no question with id - {entity.QuestionId}");

            questionToRestore.IsDeleted = false;
            _ctx.Entry(questionToRestore).State = EntityState.Modified;
        }

        public void Update(Question entity)
        {
            var existingQuestion = _ctx.Questions
                .Where(q => q.QuestionId == entity.QuestionId)
                .Include(q => q.Options)
                .SingleOrDefault();

            if (existingQuestion == null)
                throw new ArgumentException($"There is no question with id {entity.QuestionId}");

            //Update question
            _ctx.Entry(existingQuestion).CurrentValues.SetValues(entity);

            //Delete options
            foreach (var existingOption in existingQuestion.Options)
            {
                if (!entity.Options
                    .Any(o => o.OptionId == existingOption.OptionId))
                    _ctx.Options.Remove(existingOption);
            }

            var tempOptions = new List<Option>();
            foreach (var optionEntity in entity.Options)
            {
                var existingOption = existingQuestion.Options
                    .Where(o => o.OptionId == optionEntity.OptionId)
                    .SingleOrDefault();

                if(existingOption != null)
                {
                    //Update option
                    _ctx.Entry(existingOption).CurrentValues.SetValues(optionEntity);
                }
                else
                {
                    //Insert option
                    var newOption = new Option
                    {
                        Text = optionEntity.Text,
                        IsDeleted = optionEntity.IsDeleted,
                        Order = optionEntity.Order
                    };
                    tempOptions.Add(newOption);
                }
            }
            existingQuestion.Options.AddRange(tempOptions);
        }
    }
}
