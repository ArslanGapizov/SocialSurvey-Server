using SocialSurvey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Tests
{
    public class TestDataGenerator
    {
        public static User[] GenerateUsers(string name)
        {
            User[] users = new User[3];
            for (var i = 0; i < users.Length; i++)
            {
                users[i] = new User()
                {
                    Login = name + i,
                    FirstName = "first" + name + i,
                    LastName = "last" + name + i,
                    MiddleName = "middle" + name + i,
                    Role = Role.Interviewer,
                    CreationDate = new DateTime()
                };
            }
            return users;
        }
        public static Survey[] GenerateSurveys(User user)
        {
            Survey[] surveys = new Survey[3];
            for (int i = 0; i < surveys.Length; i++)
            {
                surveys[i] = new Survey()
                {
                    Name = "survey" + i + user.Login,
                    Comment = "comment",
                    User = user
                };
            }
            return surveys;
        }
        public static Question[] GenerateQuestions(Survey survey)
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
        public static Option[] GenerateOptions(Question question)
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
    }
}
