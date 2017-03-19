using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.DTO
{
    public class SurveyDTO
    {
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsDeleted { get; set; }

        public int? UserId{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<QuestionDTO> Questions { get; set; }
        public SurveyDTO()
        {
        }
    }
}
