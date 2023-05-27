using Microsoft.AspNetCore.Mvc.Filters;
using InventoryApp.DataAccess.Providers;
using InventoryApp.DataAccess.Providers.Interfaces;
using Microsoft.Net.Http.Headers;
using InventoryApp.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Request.Body.Peeker;
using System.Text;
using Microsoft.Build.Framework;

namespace InventoryApp.AppStart.Filters
{
    public class UserActionAttribute : ActionFilterAttribute
    {
        private readonly UserAuthenticationService _authenticationService;
        private readonly IUserProvider _userProvider;
        public UserActionAttribute(IUserProvider userProvider,
            UserAuthenticationService userAuthenticationService)
        {
            _authenticationService = userAuthenticationService;
            _userProvider = userProvider;
        }

        
        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var header = context.HttpContext.Request.Headers[HeaderNames.Authorization].ToArray();
            var userInfo = await _authenticationService.GetUserByHeaders(header);
            
            string email = $"user: {userInfo.Email}; ";
            string result = $"result: {context.Result}; ";
            string url = $"url: {context.HttpContext.Request.GetDisplayUrl()}; ";
            string action = $"action: {context.ActionDescriptor.DisplayName}; ";
            string message = email + result + url + action;
            
           
            LogWrite(message);
            
            await base.OnResultExecutionAsync(context, next);
        }

        private void LogWrite(string logMessage)
        {
            try
            {
                var path = System.IO.Directory.GetCurrentDirectory() + "\\" + "wwwroot\\user_logs.txt";
                var stream = new FileStream(path, FileMode.Append);
                using (StreamWriter w = new StreamWriter(stream, Encoding.UTF8))
                {
                    Log(logMessage, w);
                };
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
