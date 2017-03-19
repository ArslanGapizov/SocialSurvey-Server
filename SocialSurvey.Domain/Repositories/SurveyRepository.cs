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

            if (hard)
            {
                _ctx.Surveys.Remove(surveyToDelete);
            }
            else
            {
                surveyToDelete.IsDeleted = true;
                foreach (var question in surveyToDelete.Questions)
                {
                    question.IsDeleted = true;
                    foreach (var option in question.Options)
                        option.IsDeleted = true;
                }
                _ctx.Entry(surveyToDelete).State = EntityState.Modified;
            }
        }

        public void Delete(Survey entity, bool hard = true)
        {
            var surveyToDelete = Get(entity.SurveyId);
            if (hard)
            {
                _ctx.Surveys.Remove(surveyToDelete);
            }
            else
            {
                surveyToDelete.IsDeleted = true;
                foreach (var question in surveyToDelete.Questions)
                {
                    question.IsDeleted = true;
                    foreach (var option in question.Options)
                        option.IsDeleted = true;
                }
                _ctx.Entry(surveyToDelete).State = EntityState.Modified;
            }
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
            Survey surveyToRestore = Get(id);
            if (surveyToRestore == null)
                throw new ArgumentOutOfRangeException($"There is no survey with id - {id}");

            surveyToRestore.IsDeleted = false;
            if (surveyToRestore.Questions != null)
                foreach (var question in surveyToRestore.Questions)
                {
                    question.IsDeleted = false;
                    if (question.Options != null)
                        foreach (var option in question.Options)
                            option.IsDeleted = false;
                }

            _ctx.Entry(surveyToRestore).State = EntityState.Modified;
        }

        public void Restore(Survey entity)
        {
            var surveyToRestore = Get(entity.SurveyId);
            surveyToRestore.IsDeleted = false;
            if (surveyToRestore.Questions != null)
                foreach (var question in surveyToRestore.Questions)
                {
                    question.IsDeleted = false;
                    if (question.Options != null)
                        foreach (var option in question.Options)
                            option.IsDeleted = false;
                }

            _ctx.Entry(surveyToRestore).State = EntityState.Modified;
        }

        public void Update(Survey entity)
        {
            var existingSurvey = _ctx.Surveys
                .Where(s => s.SurveyId == entity.SurveyId)
                .Include(s => s.Questions)
                .ThenInclude(q => q.Options)
                .SingleOrDefault();

            if (existingSurvey == null)
                throw new ArgumentException($"There is no survey with id - {entity.SurveyId}");

            //Update survey
            _ctx.Entry(existingSurvey).CurrentValues.SetValues(entity);

            //Delete questions
            foreach (var existingQuestion in existingSurvey.Questions)
            {
                if (!entity.Questions
                    .Any(q => q.QuestionId == existingQuestion.QuestionId))
                    _ctx.Questions.Remove(existingQuestion);

                foreach (var existionOption in existingQuestion.Options)
                {
                    if (!entity.Questions.Any(q => q.Options.Any(o => o.OptionId == existionOption.OptionId)))
                        _ctx.Options.Remove(existionOption);
                }
            }

            //tempList of questions for adding in the end of loop
            var tempQuestions = new List<Question>();
            // Update and Insert questions
            foreach (var questionEntity in entity.Questions)
            {
                var existingQuestion = existingSurvey.Questions
                    .Where(q => q.QuestionId == questionEntity.QuestionId)
                    .SingleOrDefault();

                if (existingQuestion != null)
                {

                    //Update question
                    _ctx.Entry(existingQuestion).CurrentValues.SetValues(questionEntity);
                    
                    //tempList of options for adding in the end of loop
                    List<Option> tempOptions = new List<Option>();
                    foreach (var optionEntity in questionEntity.Options)
                    {
                        var existingOption = existingQuestion.Options
                            .Where(o => o.OptionId == optionEntity.OptionId)
                            .SingleOrDefault();

                        if (existingOption != null)
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
                else
                {
                    //Insert question
                    var newQuestion = new Question
                    {
                        Text = questionEntity.Text,
                        Order = questionEntity.Order,
                        IsDeleted = questionEntity.IsDeleted,
                        QuestionType = questionEntity.QuestionType
                    };
                    tempQuestions.Add(questionEntity);
                }
            }
            existingSurvey.Questions.AddRange(tempQuestions);
        }
    }
}
