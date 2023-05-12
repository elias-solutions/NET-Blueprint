using BIT.NET.Backend.Blueprint.Model;
using FluentAssertions;
using NUnit.Framework;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.PersonControllerTest;

public class PersonsControllerPostTest : IntegrationTestBase
{
    private const string Route = "/api/v1/persons";
    private readonly CreatePersonRequest _request;

    public PersonsControllerPostTest()
    {
        _request = new CreatePersonRequest("Jonas", "Elias", DateTime.Now);
    }

    [Test]
    public async Task PersonsController_Post_Ok()
    {
        //Act
        var response = await PostAsync<CreatePersonRequest, PersonDto>(Route, _request);

        //Assert
        response.Id.Should().NotBe(Guid.Empty);
        response.FirstName.Should().Be(_request.FirstName);
        response.LastName.Should().Be(_request.LastName);
        response.Birthday.Should().Be(_request.Birthday);
        response.Created.Should().NotBe(DateTime.MinValue);
        response.CreatedBy.Should().NotBeEmpty();
        response.Modified.Should().Be(DateTime.MinValue);
        response.ModifiedBy.Should().Be(Guid.Empty);
    }
}