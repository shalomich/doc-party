using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocParty.Filters
{
    public class SetTempDataModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var controller = filterContext.Controller as Controller;

            var errors = new List<string>();
            
            foreach (var value in controller.ViewData.ModelState.Values)
                foreach (var error in value.Errors)
                    errors.Add(error.ErrorMessage);

            controller.TempData["Errors"] = JsonConvert.SerializeObject(errors);
        }
    }
}
