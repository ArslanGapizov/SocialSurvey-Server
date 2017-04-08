using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Tests
{
    public class FormRepositoryTests
    {
        [Test]
        public void Can_add_form()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<SocialSurveyContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new SocialSurveyContext(options))
                {
                    context.Database.EnsureCreated();
                }

                User user = TestDataGenerator.GenerateUsers("name")[0];
                Survey survey = TestDataGenerator.GenerateSurveys(user)[0];
                Question question = TestDataGenerator.GenerateQuestions(survey)[0];
                user.Surveys.Add(survey);
                survey.Questions.Add(question);
                using (var context = new SocialSurveyContext(options))
                {
                    context.Add(user);
                    context.SaveChanges();
                    Assert.AreEqual(1, question.QuestionId);
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Forms.Create(new Form
                    {
                        SurveyId = 1,
                        Comments = "SomeComments",
                        StartDate = new DateTime(),
                        EndDateTime = new DateTime(),
                        Geolocation = "геолокация",
                        Answers = new List<Answer>
                        {
                            new Answer { AnswerText = "Answer1", QuestionId = 1 }
                        }
                    });
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual("SomeComments", context.Forms.Single().Comments);
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
