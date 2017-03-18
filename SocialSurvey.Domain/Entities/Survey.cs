using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialSurvey.Domain.Entities
{
    public class Survey
    {
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        public int? UserId { get; set; }
        //Creator
        public User User { get; set; }


        public List<Question> Questions { get; set; }

        public Survey()
        {
            Questions = new List<Question>();
        }
    }
}
