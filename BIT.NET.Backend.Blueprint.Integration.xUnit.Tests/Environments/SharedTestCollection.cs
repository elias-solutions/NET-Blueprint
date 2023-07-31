using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments
{
    [CollectionDefinition(nameof(SharedTestCollection))]
    internal class SharedTestCollection : ICollectionFixture<WebApplicationFactory<Startup>>
    {
    }
}
