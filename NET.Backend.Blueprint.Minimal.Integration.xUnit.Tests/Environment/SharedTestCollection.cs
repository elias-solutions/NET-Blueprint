using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<Fixture>;
