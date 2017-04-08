using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Entities
{
    public class AnswerOption
    {
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }

        public int? OptionId { get; set; }
        public Option Option { get; set; }
    }
}
