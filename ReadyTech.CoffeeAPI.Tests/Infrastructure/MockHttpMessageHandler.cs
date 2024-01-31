using Newtonsoft.Json;
using System.Net;

namespace ReadyTech.CoffeeAPI.Tests.Infrastructure
{
    internal sealed class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, HttpResponseMessage> _responses = [];
        private int _callCounter = 0;

        public bool WasCalled() => _callCounter > 0;

        public void When(string requestUri)
        {
            _responses.Add(requestUri, new HttpResponseMessage());
        }

        public void Respond(string requestUri, object? content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (!_responses.TryGetValue(requestUri, out var response))
            {
                throw new InvalidOperationException($"No setup found for {requestUri}");
            }

            response.Content = new StringContent(JsonConvert.SerializeObject(content));
            response.StatusCode = statusCode;

            _responses[requestUri] = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestUri = request.RequestUri.ToString();

            _callCounter++;

            if (_responses.TryGetValue(requestUri, out var response))
            {
                return Task.FromResult(response);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
