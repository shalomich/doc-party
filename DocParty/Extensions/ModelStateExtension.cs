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
        /// <summary>
        /// Add errors for model state and return isValid.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="responce"></param>
        /// <returns></returns>
        public static bool CheckErrors(this ModelStateDictionary modelState, ErrorResponce responce)
        {
            foreach (string message in responce.Errors)
                modelState.AddModelError("", message);

            return modelState.IsValid == false;
        }
    }
}
