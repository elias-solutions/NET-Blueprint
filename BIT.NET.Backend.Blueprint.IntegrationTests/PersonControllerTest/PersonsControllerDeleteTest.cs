using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.IntegrationTests.PersonControllerTest;

[TestFixture]
internal class PersonsControllerDeleteTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerDeleteTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task WhenRestServiceIsCalled_ThenResultShouldBeExpectedModel()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, GetPersonResponse>(Route, _request);
        var getPersonResponse = await GetAsync<IEnumerable<GetPersonResponse>>(Route);
        getPersonResponse.Should().ContainInOrder(expected);

        //Act
        await DeleteAsync($"/api/v1/persons/{expected.Id}");

        //Assert
        var response = await GetAsync<IEnumerable<GetPersonResponse>>(Route);
        response.Should().BeEmpty();
    }
}