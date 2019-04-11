using Microsoft.AspNetCore.Http;
using System;

namespace KD.CQRS.Providers
{
    public interface ICqrsProvider
    {
        bool IsCommand(HttpContext context);
        bool IsQuery(HttpContext context);
        Type GetCommandType(HttpContext context);
        Type GetQueryType(HttpContext context);
    }
}
