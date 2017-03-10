using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Entities;

namespace SocialSurvey.Domain.DB
{
    public class DBContextSeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<SocialSurveyContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var user = new User
                {
                    FirstName = "Arslan",
                    LastName = "Gapizov",
                    UserName = "AGN",
                    Role = Role.Interviewer,
                    Region = "region",
                    Area = "area",
                    Sector = "sector",
                    CreationDate = new DateTime(),

                };

                var survey = new Survey
                {
                    Name = "Is your cat a human?",
                    Comment = "just for testing",
                    User = user
                };

                survey.Questions.Add(new Question
                {
                    QuestionType = QuestionType.Select,
                    Order = 1,
                    Text = "Does your cat ever talk to you?",
                    Options = new List<Option>
                    {
                        new Option { Text = "No..mayby", Order = 1 },
                        new Option { Text = "Yes", Order = 2 },
                        new Option { Text = "Cats can`t talk, stupid", Order = 3 }
                    }
                });
                survey.Questions.Add(new Question
                {
                    QuestionType = QuestionType.Select,
                    Order = 2,
                    Text = "Does your cat have strange markings?",
                    Options = new List<Option>
                    {
                        new Option { Text = "Spectacle marks", Order = 1 },
                        new Option { Text = "Maybe", Order = 2 },
                        new Option { Text = "No", Order = 3 }
                    }
                });
                survey.Questions.Add(new Question
                {
                    QuestionType = QuestionType.Select,
                    Order = 3,
                    Text = "Is he/she a tabby?",
                    Options = new List<Option>
                    {
                        new Option { Text = "Yes", Order = 1 },
                        new Option { Text = "Yes", Order = 2 },
                        new Option { Text = "No", Order = 3 }
                    }
                });
                survey.Questions.Add(new Question
                {
                    QuestionType = QuestionType.Select,
                    Order = 4,
                    Text = "Where did you get your cat?",
                    Options = new List<Option>
                    {
                        new Option { Text = "Stray", Order = 1 },
                        new Option { Text = "Shelter", Order = 2 },
                        new Option { Text = "Breed", Order = 3 }
                    }
                });
                user.Surveys.Add(survey);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
