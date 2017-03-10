using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    interface IResponse
    {
        string Href { get; set; }
        string Message { get; set; }
    }
}
