using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anthill.API.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private string path = "errors.txt";
        public void OnException(ExceptionContext context)
        {
            string exceptionStack = context.Exception.StackTrace;
            string header = context.HttpContext.Request.Headers.ToString();
            string method = context.HttpContext.Request.Method;
            string errorMessage = context.Exception.Message;
            string name = context.ActionDescriptor.DisplayName;

            context.Result = new ContentResult
            {
                Content = "OOps something went wrong! Keep calm, all will be ok!"
            };

            string message = $" Заголовок: {header} \n Тип запроса: {method} \n В методе {name} возникло иссключение: \n {errorMessage} \n {exceptionStack}";
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(message);
                fileStream.Write(array, 0, array.Length);
            }
            context.ExceptionHandled = true;
        }
    }
}
