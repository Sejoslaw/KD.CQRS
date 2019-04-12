using Microsoft.AspNetCore.Http;
using System;

namespace KD.CQRS.Providers
{
    /// <summary>
    /// Provider which is responsible for validating and handling incoming requests.
    /// </summary>
    public interface ICqrsProvider
    {
        /// <summary>
        /// Returns true if the request is valid for executing command.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsCommand(HttpContext context);
        /// <summary>
        /// Returns true if the request is valid for executing query.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsQuery(HttpContext context);
        /// <summary>
        /// Returns type of a command to be called.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Type GetCommandType(HttpContext context);
        /// <summary>
        /// Returns type of a query to be called.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Type GetQueryType(HttpContext context);
        /// <summary>
        /// Returns parsed data from request.
        /// 
        /// NOTE: This data will be further passed as first parameter to command or query.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="messageBaseType"> Type of a model to be passed to Query or Command </param>
        /// <returns></returns>
        object GetMessageData(HttpContext context, Type messageBaseType);
    }
}
