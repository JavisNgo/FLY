using FLY.Business.Models.Account;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FLY.Business.Middlewares
{
    public class RegisterMiddleware
    {
        private readonly RequestDelegate _next;

        public RegisterMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if(context.Request.Path.StartsWithSegments("/api/v1/register") 
                && !context.Request.Path.Equals("/api/v1/register/verify") 
                && context.Request.Method == "POST")
            {
                context.Request.EnableBuffering();
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                var registerRequest = JsonConvert.DeserializeObject<RegisterRequest>(body);
                if (registerRequest != null)
                {
                    var properties = registerRequest.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(registerRequest);
                        if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync($"{property.Name} cannot be null or empty.");
                            return;
                        }
                    }
                    if (!isValidatedEmail(registerRequest.Email))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("Email is not in a correct format.");
                        return;
                    }
                    if(!isValidPhone(registerRequest.Phone))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync("Phone is not in a correct format.");
                        return;
                    }
                }
            }
            await _next(context);
        }
        private bool isValidPhone(string? phone)
        {
            var regex = new Regex(@"^(09|03|\+84)[0-9]{8}$");
            return regex.IsMatch(phone);
        }

        private bool isValidatedEmail(string emailAddress)
        {
            try
            {
                var addr = new MailAddress(emailAddress);
                return addr.Address == emailAddress;
            }
            catch
            {
                return false;
            }
        }
    }
}
