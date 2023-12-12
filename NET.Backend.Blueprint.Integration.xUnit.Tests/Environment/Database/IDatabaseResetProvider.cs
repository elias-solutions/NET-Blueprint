namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment.Database;

public interface IDatabaseResetProvider
{
    Task InitializeAsync();

    Task ResetAsync();

    Task DisposeDbConnectionAsync();
}