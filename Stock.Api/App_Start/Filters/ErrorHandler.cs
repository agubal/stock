using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Stock.Entities.Common.Utils;

namespace Stock.Api.Filters
{
    public class ErrorHandler : ExceptionFilterAttribute
    {
        /// <summary>
        /// Filter to catch all exceptions and wrap then into common response
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            string errorMessage = ErrorUtils.GetErrorMessage(context.Exception, "Service error");
            context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            base.OnException(context);
        }
    }
}