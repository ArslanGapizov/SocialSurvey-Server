using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialSurvey.Domain.DB;
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
                var newText = "newTextForQuestion";
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    surveys[0].Questions[0].Text = newText;
                    unitOfWork.Surveys.Update(surveys[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(newText, context.Questions.First().Text);
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Delete(1, false);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(true, context.Users.Single().IsDeleted);
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Delete(1, true);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(0, context.Users.Count());
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Delete(users[0], false);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(true, context.Users.Single().IsDeleted);
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Delete(users[0], true);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(0, context.Users.Count());
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    users[0].IsDeleted = true;
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Restore(1);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(false, context.Users.Single().IsDeleted);
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
                var users = TestDataGenerator.GenerateUsers("User");
                using (var context = new SocialSurveyContext(options))
                {
                    users[0].IsDeleted = true;
                    context.Users.Add(users[0]);
                    context.SaveChanges();
                }
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Restore(users[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(false, context.Users.Single().IsDeleted);
                }
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
