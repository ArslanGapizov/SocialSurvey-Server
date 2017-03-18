using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialSurvey.Domain.DB;
using SocialSurvey.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Tests
{
    public class UserRepositoryTests
    {
        [Test]
        public void Can_Create_in_UsersRepository()
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
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    unitOfWork.Users.Create(users[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual(users[0].Login, context.Users.Single().Login);
                    Assert.AreEqual(1, context.Users.Single().UserId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Get_in_UsersRepository()
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
                    Assert.AreEqual(1, unitOfWork.Users.GetAll().Count());
                    Assert.AreEqual(users[0].Login, unitOfWork.Users.Get(1).Login);
                    Assert.AreEqual(1, unitOfWork.Users.Get(1).UserId);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Test]
        public void Can_Update_in_UsersRepository()
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
                    //users[0].Login = "newLogin";
                    //context.Entry(users[0]).State = EntityState.Modified;
                    //context.SaveChanges();
                }
                var newLogin = "newLogin";
                using (var unitOfWork = new SocialSurveyUOW(options))
                {
                    users[0].Login = newLogin;
                    unitOfWork.Users.Update(users[0]);
                    unitOfWork.Save();
                }
                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(newLogin, context.Users.Single().Login);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Test]
        public void Can_DeleteById_Soft_in_UsersRepository()
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
        public void Can_DeleteById_Hard_in_UsersRepository()
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
        public void Can_DeleteByObject_Soft_in_UsersRepository()
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
        public void Can_DeleteByObject_Hard_in_UsersRepository()
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
        public void Can_RestoreById_in_UsersRepository()
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
        public void Can_RestoreByObject_in_UsersRepository()
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
