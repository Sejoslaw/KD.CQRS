using System.IO;
using System.Net;

namespace KD.CQRS.TestApp.Client
{
    internal static class Program
    {
        // TODO: Fix adding body to request.
        private static void Main(string[] args)
        {
            // Prepare request
            var request = WebRequest.Create("http://localhost:57588");

            // Specify which command to run
            request.Headers.Add("cqrs-command", "KD.CQRS.TestApp.SampleCommand");

            // Write body of the request
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes("{ \"Value\": \"14\" }");
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            // Execute command
            request.GetResponse();
        }
    }
}
