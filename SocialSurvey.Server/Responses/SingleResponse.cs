using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    public class SingleResponse<T> : ISingleResponse<T>
    {
        public string Link { get; set; }
        public string Method { get; set; }
        public string Message { get; set; }
        public T Response { get; set; }
    }
}
