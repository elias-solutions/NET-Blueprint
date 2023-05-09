using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.IntegrationTests.PersonControllerTest;

[TestFixture]
internal class PersonsControllerDeleteTest : IntegrationTestBase
{

    [Test]
    public async Task WhenRestServiceIsCalled_ThenResultShouldBeExpectedModel()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, GetPersonResponse>("/api/v1/persons", new CreatePersonRequest("Jonas", "Elias", DateTime.Now));
        var getPersonResponse = await GetAsync<IEnumerable<GetPersonResponse>>("/api/v1/persons");
        getPersonResponse.Should().ContainInOrder(expected);

        //Act
        await DeleteAsync($"/api/v1/persons/{expected.Id}");

        //Assert
        var response = await GetAsync<IEnumerable<GetPersonResponse>>("/api/v1/persons");
        response.Should().BeEmpty();
    }
}