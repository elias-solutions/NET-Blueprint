using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment
{
    [CollectionDefinition(nameof(SharedTestCollection))]
    public class SharedTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
    }
}
