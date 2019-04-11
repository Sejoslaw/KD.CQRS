using KD.CQRS.Middleware;
using Microsoft.AspNetCore.Http;
using System;
using System.Reflection;

namespace KD.CQRS.Providers
{
    /// <summary>
    /// You should specify one of the following headers in your request:
    /// 1. Header: "cqrs-command", Value: "My.Namespace.With.IMyAwesomeCommandInterfaceName"
    /// or
    /// 2. Header: "cqrs-query", Value: "My.Namespace.With.IMyAwesomeQueryInterfaceName"
    ///
    /// Also, the body of the message should be able to be cast to the input object (query or command input).
    /// </summary>
    public class DefaultCqrsProvider : ICqrsProvider
    {
        public bool IsCommand(HttpContext context)
        {
            return context.Request.Headers.ContainsKey(CqrsHeaders.CQRS_COMMAND);
        }

        public bool IsQuery(HttpContext context)
        {
            return context.Request.Headers.ContainsKey(CqrsHeaders.CQRS_QUERY);
        }

        public Type GetCommandType(HttpContext context)
        {
            string headerValue = context.Request.Headers[CqrsHeaders.CQRS_COMMAND];
            Type commandType = GetType(headerValue);
            return commandType;
        }

        public Type GetQueryType(HttpContext context)
        {
            string headerValue = context.Request.Headers[CqrsHeaders.CQRS_QUERY];
            Type queryType = GetType(headerValue);
            return queryType;
        }

        private Type GetType(string fullName)
        {
            Type messageType = Type.GetType(fullName);

            if (messageType != null)
            {
                return messageType;
            }

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.FullName.Equals(fullName))
                    {
                        return type;
                    }
                }
            }

            return messageType;
        }
    }
}
