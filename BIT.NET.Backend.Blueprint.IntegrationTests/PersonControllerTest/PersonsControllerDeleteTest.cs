using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.PersonControllerTest;

public class PersonsControllerDeleteTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerDeleteTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task PersonsController_Delete_Ok()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, PersonDto>(Route, _request);
        var getPersonResponse = await GetAsync<IEnumerable<PersonDto>>(Route);
        getPersonResponse.Should().ContainInOrder(expected);

        //Act
        await DeleteAsync($"{Route}/{expected.Id}");

        //Assert
        var response = await GetAsync<IEnumerable<PersonDto>>(Route);
        response.Should().BeEmpty();
    }
}