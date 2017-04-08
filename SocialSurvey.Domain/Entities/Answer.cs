using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }

        public int FormId { get; set; }
        public Form Form { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }


    }
}
