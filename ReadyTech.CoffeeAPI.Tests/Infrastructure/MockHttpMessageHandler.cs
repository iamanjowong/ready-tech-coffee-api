using Newtonsoft.Json;
using System.Net;

namespace ReadyTech.CoffeeAPI.Tests.Infrastructure
{
    internal sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, HttpResponseMessage> _responses = [];
        private int _callCounter = 0;

        public bool WasCalled() => _callCounter > 0;

        public void Respond(string requestUri, object? content, HttpStatusCode statusCode = HttpStatusCode.OK) =>
            _responses[requestUri] = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(content)),
                StatusCode = statusCode
            };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri!.ToString();

            _callCounter++;

            if (_responses.TryGetValue(requestUri, out var response))
            {
                return Task.FromResult(response);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
