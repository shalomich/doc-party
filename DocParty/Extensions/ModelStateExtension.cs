using DocParty.RequestHandlers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Extensions
{
    public static class ModelStateExtension
    {
        public static bool CheckErrors(this ModelStateDictionary modelState, ErrorResponce responce)
        {
            foreach (string message in responce.Errors)
                modelState.AddModelError("", message);

            return modelState.IsValid == false;
        }
    }
}
