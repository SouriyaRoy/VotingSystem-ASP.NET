using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace votingSystem.ExceptionFilter
{
    public class CanNotDeleteException : Exception
    {
        public CanNotDeleteException(string message) : base(message)
        {

        }
    }

    public class NotDeleted : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpResponseMessage message = actionExecutedContext.Request.
                CreateResponse(HttpStatusCode.BadRequest, actionExecutedContext.Exception.Message);

            actionExecutedContext.Response = message;
        }
    }
}