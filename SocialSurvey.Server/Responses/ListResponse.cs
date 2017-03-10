using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    public class ListResponse<T> : IListResponse<T>
    {
        public string Href { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Response { get; set; }
    }
}
