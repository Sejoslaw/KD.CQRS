using KD.CQRS.Core;
using KD.CQRS.Middleware;
using KD.CQRS.Providers;
using KD.CQRS.UnitTests.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace KD.CQRS.UnitTests
{
    public class SampleQueryModel
    {
        public int Value { get; set; }
    }

    public interface ISampleQuery : IQuery<SampleQueryModel, bool>
    {
    }

    public interface ISampleCommand : ICommand<SampleQueryModel>
    {
    }

    public class SampleQuery : ISampleQuery
    {
        public bool Execute(SampleQueryModel input)
        {
            return input.Value % 2 == 0;
        }
    }

    public class SampleCommand : ISampleCommand
    {
        public object Execute(SampleQueryModel source)
        {
            Console.WriteLine(source.Value);
            return null;
        }
    }

    public class TestCqrsClassRetrieving
    {
        private IServiceProvider ServiceProvider { get; }

        public TestCqrsClassRetrieving()
        {
            IServiceCollection col = new ServiceCollection();
            col.AddSingleton<ISampleQuery, SampleQuery>();
            col.AddSingleton<ISampleCommand, SampleCommand>();

            ServiceProvider = col.BuildServiceProvider();
        }

        [Fact]
        public void Try_call_sample_query_should_return_true()
        {
            var fullName = typeof(ISampleQuery).FullName;
            var middleware =
                new CqrsMiddleware(null, (authContext => true), new DefaultCqrsProvider(), ServiceProvider);

            var context = new DefaultHttpContext();
            context.Request.Headers.Add(CqrsHeaders.CQRS_QUERY, fullName);
            context.Request.Body = "{ \"Value\": \"14\" }".ToStream();

            middleware.Invoke(context).Wait();
        }

        [Fact]
        public void Try_call_sample_command_should_return_true()
        {
            var fullName = typeof(ISampleCommand).FullName;
            var middleware =
                new CqrsMiddleware(null, (authContext => true), new DefaultCqrsProvider(), ServiceProvider);

            var context = new DefaultHttpContext();
            context.Request.Headers.Add(CqrsHeaders.CQRS_COMMAND, fullName);
            context.Request.Body = "{ \"Value\": \"14\" }".ToStream();

            middleware.Invoke(context).Wait();
        }
    }
}
