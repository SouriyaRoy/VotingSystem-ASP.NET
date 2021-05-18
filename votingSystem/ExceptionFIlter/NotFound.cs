using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace votingSystem.ExceptionFilter
{
    public class CandidateNotFoundException : Exception
    {
        public CandidateNotFoundException(string message) : base(message)
        {

        }
    }

    public class PollNotFoundException : Exception
    {
        public PollNotFoundException(string message) : base(message)
        {

        }
    }

    public class OptionNotFoundException : Exception
    {
        public OptionNotFoundException(string message) : base(message)
        {

        }
    }

    public class NotFound : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpResponseMessage message = actionExecutedContext.Request.
                CreateResponse(HttpStatusCode.NotFound, actionExecutedContext.Exception.Message);

            actionExecutedContext.Response = message;
        }
    }
}