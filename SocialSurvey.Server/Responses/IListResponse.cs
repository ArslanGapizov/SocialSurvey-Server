using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSurvey.Server.Responses
{
    interface IListResponse<T> : IResponse
    {
        IEnumerable<T> Response { get; set; }
    }
}
