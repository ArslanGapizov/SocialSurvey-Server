﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Entities;
using SocialSurvey.Domain.Repositories;

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

                User user = TestDataGenerator.GenerateUsers("name")[0];
                string userName = user.Login;
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(userName, context.Users.Single().Login);
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

                User user = TestDataGenerator.GenerateUsers("name")[0];
                Survey survey = TestDataGenerator.GenerateSurveys(user)[0];
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


                User user = TestDataGenerator.GenerateUsers("name")[0];
                Survey survey = TestDataGenerator.GenerateSurveys(user)[0];
                Question question = TestDataGenerator.GenerateQuestions(survey)[0];
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

                User user = TestDataGenerator.GenerateUsers("name")[0];
                Survey survey = TestDataGenerator.GenerateSurveys(user)[0];
                Question question = TestDataGenerator.GenerateQuestions(survey)[0];
                Option option = TestDataGenerator.GenerateOptions(question)[0];
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
    }
}
