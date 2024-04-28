using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;

namespace first_exam.Models
{
    public class TimeElapsed : Attribute, IActionFilter
    {
        private Stopwatch timer;
        private readonly ILogger<TimeElapsed> _logger;
        public TimeElapsed()
        {

        }
        public TimeElapsed(ILogger<TimeElapsed> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            timer.Stop();

            string result = "ElapsedTime: " +
                            $"{timer.ElapsedMilliseconds} ms";

            string actionName = context.ActionDescriptor.DisplayName;
            string controller = "Home";
            //_logger.LogInformation("метод действия = {actionName} {ElapsedTime}", actionName, timer.ElapsedMilliseconds);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            timer = Stopwatch.StartNew();

        }
    }
}
