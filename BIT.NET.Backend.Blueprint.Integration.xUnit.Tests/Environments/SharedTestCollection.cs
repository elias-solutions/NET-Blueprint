using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments
{
    [CollectionDefinition("Test collection")]
    internal class SharedTestCollection : ICollectionFixture<WebApplicationFactory<Startup>>
    {
    }
}
