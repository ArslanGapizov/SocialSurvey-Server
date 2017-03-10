using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    interface ISingleResponse<T> : IResponse
    {
        T Response { get; set; }
    }
}
