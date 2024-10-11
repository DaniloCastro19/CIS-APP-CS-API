using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cis_api_legacy_integration_phase_2.Src.Core.Validations
{
    public class ExceptionFilter : IActionFilter
    {
        /// <summary>
        /// This method is executed after the action method is invoked. It checks if the model state is valid.
        /// If not, it extracts the error messages from the model state and returns them in a BadRequestObjectResult.
        /// </summary>
        /// <param name="context">The context for the action executed event.</param>
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
