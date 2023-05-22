using System.Net;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;
using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;
using Xunit;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Api.V1.PersonsControllerGetAllTests;

public class GetAllOkEmptyElementsTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";

    public GetAllOkEmptyElementsTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected override Task InitAsync() => Task.CompletedTask;

    protected override Task DeInitAsync() => Task.CompletedTask;

    [Fact]
    public async Task PersonController_GetAll_Ok()
    {
        var result = await AssertGetAsync<IEnumerable<PersonDto>>(TestUsers.Admin, Route);
        result.Should().BeEmpty();
    }
}