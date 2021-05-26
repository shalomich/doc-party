using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace DocParty.Filters
{
    /// <summary>
    /// Filter that get errors from tempdata after redirect.
    /// Errors were be added by filter SetTempDataModelState.
    /// </summary>
    public class RestoreModelStateFromTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controller = filterContext.Controller as Controller;

            if (controller.TempData.ContainsKey("Errors"))
            {
                var errors = JsonConvert.DeserializeObject<List<string>>((string)controller.TempData["Errors"]);

                foreach (var error in errors)
                    controller.ViewData.ModelState.AddModelError("", error);
            }
        }
    }
}
