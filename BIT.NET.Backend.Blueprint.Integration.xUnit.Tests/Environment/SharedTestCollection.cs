using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environment
{
    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
    }
}
