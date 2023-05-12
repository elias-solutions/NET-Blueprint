using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.PersonControllerTest;

public class PersonsControllerGetAllTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerGetAllTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task PersonsController_GetAll_Ok()
    {
        //Arrange
        var expected = await PostAsync<CreatePersonRequest, PersonDto>(Route, _request);

        //Act
        var response = await GetAsync<IEnumerable<PersonDto>>(Route);
            
        //Assert
        response.Should().BeEquivalentTo(new[] { expected });
    }
}