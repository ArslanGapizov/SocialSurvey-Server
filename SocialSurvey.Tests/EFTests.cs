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

                using (var context = new SocialSurveyContext(options))
                {
                    context.Users.Add(new User()
                    {
                        UserName = "AAA",
                        FirstName = "AAA",
                        LastName = "BBB",
                        MiddleName = "CCC",
                        Area = "Area",
                        Region = "Region",
                        Role = Role.Interviewer,
                        Sector = "Sector",
                        CreationDate = new DateTime()
                    });
                    context.SaveChanges();
                }

                using (var context = new SocialSurveyContext(options))
                {
                    Assert.AreEqual(1, context.Users.Count());
                    Assert.AreEqual("AAA", context.Users.Single().UserName);
                    Assert.AreEqual(1, context.Users.Single().UserId);
                }
            }

            finally
            {
                connection.Close();
            }
        }
    }
}
