using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Mottu.Api.Tests
{
    public class HealthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HealthTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var resp = await client.GetAsync("/health");
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
