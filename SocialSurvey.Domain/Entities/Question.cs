using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Entities
{
    public enum QuestionType : byte
    {
        Select,
        MultiSelect,
        Text
    }
    public class Question
    {
        public int QuestionId { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public List<Option> Options { get; set; }

        public Question()
        {
            Options = new List<Option>();
        }
    }
}
