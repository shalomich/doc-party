using DocParty.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DocParty.Filters
{
    /// <summary>
    /// Filter that handle NotFoundException and return 404 status.
    /// </summary>
    public class NotFoundPageFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
                context.Result = new NotFoundResult();
            context.ExceptionHandled = true;
        }
    }
}
