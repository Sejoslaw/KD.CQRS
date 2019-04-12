using KD.CQRS.Core;
using KD.CQRS.Providers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KD.CQRS.Middleware
{
    /// <summary>
    /// Middleware responsible for executing appropriate command or query.
    /// </summary>
    public class CqrsMiddleware
    {
        /// <summary>
        /// Next request handler in the pipeline.
        /// </summary>
        private RequestDelegate Next { get; }
        /// <summary>
        /// Function responsible for authorizing context.
        /// </summary>
        private Func<HttpContext, bool> AuthorizeFunc { get; }
        /// <summary>
        /// Provider which is responsible for retrieve command or query.
        /// </summary>
        private ICqrsProvider CqrsProvider { get; }
        /// <summary>
        /// Contains registered commands and queries.
        /// </summary>
        private IServiceProvider ServiceProvider { get; }

        public CqrsMiddleware(RequestDelegate next, Func<HttpContext, bool> authorizeFunc, ICqrsProvider cqrsProvider, IServiceProvider serviceProvider)
        {
            Next = next;
            AuthorizeFunc = authorizeFunc;
            CqrsProvider = cqrsProvider;
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Check context and execute appropriate command or query.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (!AuthorizeFunc(httpContext))
            {
                await Next(httpContext);
                return;
            }

            // Command or query type
            Type messageType = null;

            if (CqrsProvider.IsCommand(httpContext))
            {
                messageType = CqrsProvider.GetCommandType(httpContext);
            }

            if (CqrsProvider.IsQuery(httpContext))
            {
                messageType = CqrsProvider.GetQueryType(httpContext);
            }

            if (messageType == null)
            {
                throw new ArgumentException($"Cannot read query nor command name from HTTP context.", nameof(httpContext));
            }

            // Get command or query from container
            object message = ServiceProvider.GetService(messageType);

            // Base type (IQuery or ICommand)
            Type messageBaseType = FindBaseType(messageType);

            // Get data object from request body
            // 0-index element will always be an input to query or command
            object data = GetData(httpContext.Request.Body, messageBaseType.GetGenericArguments()[0]);

            // Execute command or query
            messageBaseType.GetMethod("Execute").Invoke(message, new object[] { data });
        }

        private object GetData(Stream requestBody, Type messageBaseType)
        {
            var reader = new StreamReader(requestBody);
            string body = reader.ReadToEnd();
            object data = JsonConvert.DeserializeObject(body, messageBaseType);
            return data;
        }

        private Type FindBaseType(Type messageType)
        {
            if ((messageType.IsGenericType && messageType.GetGenericTypeDefinition() == typeof(IQuery<,>)) ||
                (messageType.IsGenericType && messageType.GetGenericTypeDefinition() == typeof(ICommand<>)))
            {
                return messageType;
            }

            foreach (Type baseInterfaces in messageType.GetInterfaces())
            {
                messageType = FindBaseType(baseInterfaces);

                if (messageType != null)
                {
                    return messageType;
                }
            }

            return null;
        }
    }
}
