using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.IntegrationTests.PersonControllerTest;

[TestFixture]
internal class PersonsControllerGetAllTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerGetAllTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task WhenRestServiceIsCalled_ThenResultShouldBeExpectedModelList()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, GetPersonResponse>(Route, _request);

        //Act
        var response = await GetAsync<IEnumerable<GetPersonResponse>>(Route);
            
        //Assert
        response.Should().BeEquivalentTo(new[] { expected });
    }
}