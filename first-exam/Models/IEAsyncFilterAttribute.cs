using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace first_exam.Models
{
    public class IEAsyncFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            //получаем информаиюо браузере пользователя
            string userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            if (Regex.IsMatch(userAgent, "MSIE|Trident"))
            {
                context.Result = new ContentResult { Content = "Ваш браузер устарел" };
            }
            else //если браузер не IE  передаем запросдальше
                await next();
        }
    }

    public class IEFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //получаем информаиюо браузере пользователя
            string userAgent = context.HttpContext
                .Request
                .Headers["User-Agent"].ToString();

            if (Regex.IsMatch(userAgent, "Mozilla"))
            {
                context.Result = new ContentResult
                {
                    Content = "Ваш браузер устарел"
                };
            }
        }
    }
}
