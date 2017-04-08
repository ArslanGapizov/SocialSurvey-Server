using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Domain.Entities
{
    public class Form
    {
        public int FromId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Geolocation { get; set; }
        public string Comments { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

        public List<Answer> Answers { get; set; }

        public Form()
        {
            Answers = new List<Answer>();
        }
    }
}
