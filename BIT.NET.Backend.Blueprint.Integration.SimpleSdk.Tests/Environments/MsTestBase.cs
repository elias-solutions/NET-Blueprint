using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIT.NET.Backend.Blueprint.Integration.SimpleSdk.Tests.Environments
{
    [TestClass]
    public abstract class MsTestBase : IntegrationTestBase<Startup>
    {
        [AssemblyInitialize]
        public static Task Init(TestContext testContext)
        {
            return AssemblyInitializeAsync(testContext, (services, configuration) => { });
        }

        [AssemblyCleanup]
        public static void Cleanup() => AssemblyCleanup();
    }
}
