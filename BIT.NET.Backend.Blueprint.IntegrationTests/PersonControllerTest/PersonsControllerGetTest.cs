using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.IntegrationTests.PersonControllerTest;

[TestFixture]
internal class PersonsControllerGetTest : IntegrationTestBase
{
    private readonly CreatePersonRequest _request;

    public PersonsControllerGetTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task WhenRestServiceIsCalled_ThenResultShouldBeExpectedModel()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, GetPersonResponse>("/api/v1/persons", _request);

        //Act
        var response = await GetAsync<GetPersonResponse>($"/api/v1/persons/{expected.Id}");

        //Assert
        response.Should().BeEquivalentTo(expected);
    }
}