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
    public class SurveyRepositoryTests
    {

        [Test]
        public void Can_Create_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateSurveys();
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Create(surveys[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Surveys.Count());
                    Assert.AreEqual(surveys[0].Name, context.Surveys.Single().Name);
                    Assert.AreEqual(1, context.Surveys.Single().SurveyId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Get_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    Debug.WriteLine(surveys[0].Name);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    Assert.AreEqual(1, unitOfWork.Surveys.GetAll().Count());
                    Assert.AreEqual(surveys[0].Name, unitOfWork.Surveys.Get(1).Name);
                    Assert.AreEqual(1, unitOfWork.Surveys.Get(1).SurveyId);
                    Assert.IsNotNull(unitOfWork.Surveys.Get(1).Questions);
                    Assert.IsNotNull(unitOfWork.Surveys.Get(1).Questions[0].Options);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Update_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                //New Text for first question
                var newText = "newTextForQuestion";
                Option newOption = new Option
                {
                    Text = "newOption",
                    IsDeleted = false,
                    Order = 5
                };

                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    surveys[0].Questions[0].Text = newText;
                    surveys[0].Questions[1].Options.Add(newOption);
                    surveys[0].Questions[2].IsDeleted = true;
                    unitOfWork.Surveys.Update(surveys[0]);
                    unitOfWork.Save();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    var survey = unitOfWork.Surveys.Get(1);
                    Assert.AreEqual(newText, survey.Questions[0].Text);
                    Assert.AreEqual(newOption.Text, survey.Questions[1].Options.Last().Text);
                    Assert.AreEqual(true, survey.Questions[2].IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_DeleteById_Soft_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(1, false);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(true, context.Surveys.Single().IsDeleted);
                    Assert.AreEqual(true, context.Questions.First().IsDeleted);
                    Assert.AreEqual(true, context.Options.First().IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_DeleteById_Hard_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(1, true);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(0, context.Surveys.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_DeleteByObject_Soft_in__SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(surveys[0], false);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(true, context.Surveys.Single().IsDeleted);
                    Assert.AreEqual(true, context.Questions.First().IsDeleted);
                    Assert.AreEqual(true, context.Options.First().IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_DeleteByObject_Hard_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(surveys[0], true);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(0, context.Surveys.Count());
                    Assert.AreEqual(0, context.Questions.Count());
                    Assert.AreEqual(0, context.Options.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_RestoreById_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(1, false);
                    unitOfWork.Save();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Restore(1);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(false, context.Surveys.Single().IsDeleted);
                    Assert.AreEqual(false, context.Questions.First().IsDeleted);
                    Assert.AreEqual(false, context.Options.First().IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_RestoreByObject_in_SurveyRepository()
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
                var surveys = TestDataGenerator.GenerateFullSurveys();
                using (var context = new SocialSurveyContext(options))
                {
                    context.Surveys.Add(surveys[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Delete(surveys[0], false);
                    unitOfWork.Save();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Surveys.Restore(surveys[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(false, context.Surveys.Single().IsDeleted);
                    Assert.AreEqual(false, context.Questions.First().IsDeleted);
                    Assert.AreEqual(false, context.Options.First().IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
