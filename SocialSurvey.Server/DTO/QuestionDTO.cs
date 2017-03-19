using SocialSurvey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SocialSurvey.Server.DTO
{
    public class QuestionDTO
    {
        public int QuestionId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public QuestionType QuestionType { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int SurveyId { get; set; }
        public List<OptionDTO> Options { get; set; }
        
    }
}
