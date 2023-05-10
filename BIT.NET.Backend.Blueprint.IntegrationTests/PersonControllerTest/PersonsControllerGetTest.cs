using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.PersonControllerTest;

public class PersonsControllerGetTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerGetTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task PersonsController_Get_Ok()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, GetPersonResponse>(Route, _request);

        //Act
        var response = await GetAsync<GetPersonResponse>($"{Route}/{expected.Id}");

        //Assert
        response.Should().BeEquivalentTo(expected);
    }
}