using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace votingSystem.ExceptionFilter
{
    public class CanNotUpdateException : Exception
    {
        public CanNotUpdateException(string message) : base(message)
        {

        }
    }

    public class NotUpdate : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpResponseMessage message = actionExecutedContext.Request.
                CreateResponse(HttpStatusCode.BadRequest, actionExecutedContext.Exception.Message);

            actionExecutedContext.Response = message;
        }
    }
}