using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace votingSystem.ExceptionFilter
{
    public class WrongDataException : Exception
    {
        public WrongDataException(string message) : base(message)
        {

        }
    }

    public class NotAdded : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpResponseMessage message = actionExecutedContext.Request.
                CreateResponse(HttpStatusCode.BadRequest, actionExecutedContext.Exception.Message);

            actionExecutedContext.Response = message;
        }
    }
}