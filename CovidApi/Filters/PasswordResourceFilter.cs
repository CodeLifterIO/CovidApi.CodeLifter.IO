using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CovidApi.Filters
{
    public class PasswordResourceFilter : Attribute, IResourceFilter
    {
        // TODO - This cannot be the best way to only hit one controller
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            string enteredPassword;
            enteredPassword = context.HttpContext.Request.Query["password"].ToString();
            string storedPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (!string.IsNullOrWhiteSpace(storedPassword) && storedPassword != enteredPassword && context.HttpContext.Request.Path.StartsWithSegments(new PathString("/admin")))
            {
                context.Result = new ContentResult()
                {
                    Content = "There is a password set and your input does not match.",
                    StatusCode = 401
                };
            }

        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
