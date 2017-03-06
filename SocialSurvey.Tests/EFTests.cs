using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Entities;

namespace SocialSurvey.Tests
{
    public class EFTests
    {
        #region Can add entities
        [Test]
        public void Can_Add_User()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<SocialSurveyContext>()
                    .UseSqlite(connection)
                    .Options;

                //Create the schema in the database

                using (var context = new SocialSurveyContext(options))
                {
                    context.Database.EnsureCreated();
                }

                //Run the test against one instance of the context

                User user = GenerateUsers("name")[0];
                string userName = user.UserName;
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(userName, context.Users.Single().UserName);
                    Assert.AreEqual(1, context.Users.Single().UserId);
                }
            }

            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Add_Survey()
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

                User user = GenerateUsers("name")[0];
                Survey survey = GenerateSurveys(user)[0];
                string surveyName = survey.Name;
                using (var context = new SocialSurveyContext(options))
                {
                    
                    context.Users.Add(user);
                    context.Surveys.Add(survey);
                    context.SaveChanges();
                }

                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Surveys.Count());
                    Assert.AreEqual(surveyName, context.Surveys.Single().Name);
                    Assert.AreEqual(1, context.Surveys.Single().SurveyId);

                    Assert.AreEqual(1, context.Surveys.Single().UserId);
                }
            }
            finally
            {
                connection.Close();
            }

        }
        [Test]
        public void Can_Add_Question()
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


                User user = GenerateUsers("name")[0];
                Survey survey = GenerateSurveys(user)[0];
                Question question = GenerateQuestions(survey)[0];
                string questionText = question.Text;
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(user);
                    context.Surveys.Add(survey);
                    context.Questions.Add(question);
                    context.SaveChanges();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Questions.Count());
                    Assert.AreEqual(questionText, context.Questions.Single().Text);
                    Assert.AreEqual(1, context.Questions.Single().QuestionId);

                    Assert.AreEqual(1, context.Questions.Single().SurveyId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Add_Option()
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

                User user = GenerateUsers("name")[0];
                Survey survey = GenerateSurveys(user)[0];
                Question question = GenerateQuestions(survey)[0];
                Option option = GenerateOptions(question)[0];
                string optionText = option.Text;
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(user);
                    context.Surveys.Add(survey);
                    context.Questions.Add(question);
                    context.Options.Add(option);
                    context.SaveChanges();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Options.Count());
                    Assert.AreEqual(optionText, context.Options.Single().Text);
                    Assert.AreEqual(1, context.Options.Single().OptionId);

                    Assert.AreEqual(1, context.Options.Single().QuestionId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        [Test]
        public void Can_Get_Users()
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

                User[] users = GenerateUsers("name");
                using (var context = new SocialSurveyContext(options))
                {
                    foreach (var user in users)
                    {
                        context.Users.Add(user);
                    }
                    context.SaveChanges();
                }
                using (var repository = new SocialSurveyRepository(new SocialSurveyContext(options)))
                {
                    var result = repository.GetUsers();

                    Assert.AreEqual(3, result.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public void Can_Find_Users()
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

            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Find_Surveys()
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

            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Find_Questions()
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

            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Find_Options()
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

            }
            finally
            {
                connection.Close();
            }
        }

        #region Methods for generate entities
        private User[] GenerateUsers(string name)
        {
            User[] users = new User[3];
            for (var i = 0; i < users.Length; i++)
            {
                users[i] = new User()
                {
                    UserName = name + i,
                    FirstName = "first" + name + i,
                    LastName = "last" + name + i,
                    MiddleName = "middle" + name + i,
                    Area = "Area",
                    Region = "Region",
                    Role = Role.Interviewer,
                    Sector = "Sector",
                    CreationDate = new DateTime()
                };
            }
            return users;
        }
        private Survey[] GenerateSurveys(User user)
        {
            Survey[] surveys = new Survey[3];
            for (int i = 0; i < surveys.Length; i++)
            {
                surveys[i] = new Survey()
                {
                    Name = "survey" + i + user.UserName,
                    Comment = "comment",
                    User = user
                };
            }
            return surveys;
        }
        private Question[] GenerateQuestions(Survey survey)
        {
            Question[] questions = new Question[3];
            for (int i = 0; i < questions.Length; i++)
            {
                questions[i] = new Question()
                {
                    Text = "question" + i + survey.Name,
                    QuestionType = QuestionType.Select,
                    Order = i,
                    Survey = survey
                };
            }
            return questions;
        }
        private Option[] GenerateOptions(Question question)
        {
            Option[] options = new Option[3];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new Option()
                {
                    Text = "option" + i + question.Text,
                    Order = i,
                    Question = question
                };
            }
            return options;
        }
        #endregion
    }
}
