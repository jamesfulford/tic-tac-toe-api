using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace TicTacToe {
    /// <summary>
    /// Catches and handles all exceptions thrown triggered by processing requests.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter" />
    public class GlobalExceptionHandler : IExceptionFilter {
        /// <summary>
        /// Called after an action has thrown an <see cref="T:System.Exception" />.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ExceptionContext" />.</param>
        public void OnException (ExceptionContext context) {
            HttpResponse httpResponse = context.HttpContext.Response;
            httpResponse.ContentType = "application/json";

            Models.ErrorResponse errorResponse;

            if (context.Exception is ArgumentException) {
                httpResponse.StatusCode = (int) HttpStatusCode.BadRequest;
                errorResponse = new Models.ErrorResponse (1, context.Exception.Message);
            } else {
                httpResponse.StatusCode = (int) HttpStatusCode.InternalServerError;
                errorResponse = new Models.ErrorResponse (0, $"An internal error has occured: {context.Exception.Message}");
            }

            // Serialize error response into json
            string jsonResult = JsonConvert.SerializeObject (errorResponse, Formatting.Indented);

            // Indicate that the exception has been handled
            context.ExceptionHandled = true;

            // Write the error response
            httpResponse.WriteAsync (jsonResult).ConfigureAwait (false);
        }
    }
}
