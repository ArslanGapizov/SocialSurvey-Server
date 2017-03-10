using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.DTO
{
    public class OptionDTO
    {
        public int OptionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int QuestionId { get; set; }
    }
}
