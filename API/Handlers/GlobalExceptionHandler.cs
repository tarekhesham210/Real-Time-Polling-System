using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            
            var response = new
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Something went wrong",
                Message = "Unexpected error occurred. Our team is working on it."
            };

            httpContext.Response.StatusCode = response.Status;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true; 
        }
    }
}
