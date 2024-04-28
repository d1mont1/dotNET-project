using Microsoft.AspNetCore.Mvc.Filters;

namespace first_exam.Models
{
    public class AddCustomHeaderResourceFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // Код, выполняемый перед действием
            context.HttpContext
                .Response
                .Headers
                .Add("X-Custom-Header", "MyCustomValue");
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Код, выполняемый после действия
        }
    }
}
