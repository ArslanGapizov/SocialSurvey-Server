using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Entities
{
    public class Option
    {
        public int OptionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
