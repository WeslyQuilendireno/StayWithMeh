using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using Xunit;

namespace StayWithMeh.Tests
{
    public class RouteTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RouteTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Explore")]
        [InlineData("/Booking/MyBookings")]
        [InlineData("/Room")]
        [InlineData("/Wishlist")]
        [InlineData("/Support")]
        public async Task Pages_Return200(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}