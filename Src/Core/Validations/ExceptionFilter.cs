using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cis_api_legacy_integration_phase_2.Src.Core.Validations
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                context.Result = new BadRequestObjectResult(new { Errors = errors });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context){}
    }
}
