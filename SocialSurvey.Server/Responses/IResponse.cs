using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    interface IResponse
    {
        string Link { get; set; }
        string Message { get; set; }
    }
}
