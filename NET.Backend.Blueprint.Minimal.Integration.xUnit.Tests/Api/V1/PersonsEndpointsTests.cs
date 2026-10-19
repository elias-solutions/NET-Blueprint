using System.Net;
using Microsoft.AspNetCore.Mvc;
using NET.Backend.Blueprint.Application.CQRS.Commands;
using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Application.Model.Queries;
using NET.Backend.Blueprint.Domain.Enums;
using NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Api.V1;

[Collection(nameof(SharedTestCollection))]
public class PersonsEndpointsTests(Fixture fixture) : IAsyncLifetime
{
    private const string Route = "/api/v1/persons";

    public Task InitializeAsync() => fixture.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    public static TheoryData<string, string> Routes => new()
    {
        { "GET", Route },
        { "GET", $"{Route}/{Guid.NewGuid()}" },
        { "POST", Route },
        { "PUT", $"{Route}/{Guid.NewGuid()}" },
        { "DELETE", $"{Route}/{Guid.NewGuid()}" }
    };

    public static TheoryData<string, string> WriteRoutes => new()
    {
        { "POST", Route },
        { "PUT", $"{Route}/{Guid.NewGuid()}" },
        { "DELETE", $"{Route}/{Guid.NewGuid()}" }
    };

    [Theory]
    [MemberData(nameof(Routes))]
    public async Task Persons_WithoutUser_Unauthorized(string method, string route)
    {
        var response = await fixture.SendAsync(new HttpMethod(method), route, null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(WriteRoutes))]
    public async Task Persons_WriteWithStandardUser_Forbidden(string method, string route)
    {
        var response = await fixture.SendAsync(new HttpMethod(method), route, TestUsers.Standard);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WithStandardUser_Ok()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, Route, TestUsers.Standard);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithStandardUser_Ok()
    {
        var id = await CreatePersonAsync(TestData.CreatePersonRequest());

        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{id}", TestUsers.Standard);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_ValidPerson_Ok()
    {
        var response = await fixture.SendAsync(HttpMethod.Post, Route, TestUsers.Admin, TestData.CreatePersonRequest());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var created = await response.ReadAsync<CreatePersonResponse>();
        Assert.NotEqual(Guid.Empty, created.Id);
    }

    [Fact]
    public async Task Post_MissingFirstName_BadRequest()
    {
        var request = TestData.CreatePersonRequest() with { FirstName = null! };

        var response = await fixture.SendAsync(HttpMethod.Post, Route, TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_WithoutOptionalFields_Ok()
    {
        var request = TestData.CreatePersonRequest() with
        {
            Birthday = null, Gender = null, Description = null, Email = null, Phone = null, Type = null
        };
        var id = await CreatePersonAsync(request);

        var person = await GetPersonAsync(id);

        Assert.Null(person.Birthday);
        Assert.Null(person.Gender);
        Assert.Null(person.Description);
        Assert.Null(person.Email);
        Assert.Null(person.Phone);
        Assert.Null(person.Type);
    }

    [Fact]
    public async Task Post_InvalidEmail_BadRequest()
    {
        var request = TestData.CreatePersonRequest() with { Email = "no-email" };

        var response = await fixture.SendAsync(HttpMethod.Post, Route, TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_ExistingPerson_Ok()
    {
        var request = TestData.CreatePersonRequest();
        var id = await CreatePersonAsync(request);

        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{id}", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var person = await response.ReadAsync<GetPersonResponse>();
        Assert.Equal(id, person.Id);
        Assert.Equal(request.FirstName, person.FirstName);
        Assert.Equal(request.LastName, person.LastName);
        Assert.Equal(request.Birthday, person.Birthday);
        Assert.Equal(request.Gender, person.Gender);
        Assert.Equal(request.Description, person.Description);
        Assert.Equal(request.Email, person.Email);
        Assert.Equal(request.Phone, person.Phone);
        Assert.Equal(request.Type, person.Type);
        Assert.Equal(request.Addresses.Length, person.Addresses.Count());
        Assert.NotEqual(Guid.Empty, person.Version);
    }

    [Fact]
    public async Task GetById_UnknownPerson_BadRequestProblemDetails()
    {
        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{Guid.NewGuid()}", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetAll_TwoPersons_Ok()
    {
        await CreatePersonAsync(TestData.CreatePersonRequest());
        await CreatePersonAsync(TestData.CreatePersonRequest());

        var response = await fixture.SendAsync(HttpMethod.Get, Route, TestUsers.Admin);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var persons = await response.ReadAsync<GetPersonResponse[]>();
        Assert.Equal(2, persons.Length);
    }

    [Fact]
    public async Task Put_UpdatesPersonAndAddresses_Ok()
    {
        var id = await CreatePersonAsync(TestData.CreatePersonRequest());
        var person = await GetPersonAsync(id);
        var keptAddress = person.Addresses.First();
        var request = ToUpdateRequest(person, [
            ToUpdateAddressRequest(keptAddress) with { Street = "Updated Street" },
            new UpdateAddressRequest(Guid.Empty, "New Street", "9", "Bern", "3000", TestUsers.Admin.Id,
                TestData.CreatedDate, TestUsers.Admin.Id, TestData.ModifiedDate, Guid.Empty)
        ]);

        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{id}", TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updated = await GetPersonAsync(id);
        Assert.Equal("Joe", updated.FirstName);
        Assert.Equal(Gender.Female, updated.Gender);
        Assert.Equal(PersonType.Freelancer, updated.Type);
        Assert.Equal("joe.el@example.com", updated.Email);
        Assert.NotEqual(person.Version, updated.Version);
        Assert.Equal(2, updated.Addresses.Count());
        Assert.Contains(updated.Addresses, a => a.Id == keptAddress.Id && a.Street == "Updated Street");
        Assert.Contains(updated.Addresses, a => a.Street == "New Street");
    }

    [Fact]
    public async Task Put_StaleVersion_BadRequest()
    {
        var id = await CreatePersonAsync(TestData.CreatePersonRequest());
        var person = await GetPersonAsync(id);
        var request = ToUpdateRequest(person, []) with { Version = Guid.NewGuid() };

        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{id}", TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var problem = await response.ReadAsync<ProblemDetails>();
        Assert.Equal("BadRequest - Entity version conflict", problem.Title);
        Assert.Equal("Entity has been updated through other user.", problem.Detail);
    }

    [Fact]
    public async Task Put_RouteIdDiffersFromBodyId_BadRequest()
    {
        var id = await CreatePersonAsync(TestData.CreatePersonRequest());
        var person = await GetPersonAsync(id);
        var request = ToUpdateRequest(person, []);

        var response = await fixture.SendAsync(HttpMethod.Put, $"{Route}/{Guid.NewGuid()}", TestUsers.Admin, request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingPerson_Ok()
    {
        var id = await CreatePersonAsync(TestData.CreatePersonRequest());

        var response = await fixture.SendAsync(HttpMethod.Delete, $"{Route}/{id}", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{id}", TestUsers.Admin);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_UnknownPerson_BadRequest()
    {
        var response = await fixture.SendAsync(HttpMethod.Delete, $"{Route}/{Guid.NewGuid()}", TestUsers.Admin);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task<Guid> CreatePersonAsync(CreatePersonRequest request)
    {
        var response = await fixture.SendAsync(HttpMethod.Post, Route, TestUsers.Admin, request);
        response.EnsureSuccessStatusCode();
        return (await response.ReadAsync<CreatePersonResponse>()).Id;
    }

    private async Task<GetPersonResponse> GetPersonAsync(Guid id)
    {
        var response = await fixture.SendAsync(HttpMethod.Get, $"{Route}/{id}", TestUsers.Admin);
        response.EnsureSuccessStatusCode();
        return await response.ReadAsync<GetPersonResponse>();
    }

    private static UpdatePersonRequest ToUpdateRequest(GetPersonResponse person, UpdateAddressRequest[] addresses) => new(
        person.Id, "Joe", "El", new DateOnly(2020, 5, 3),
        Gender.Female, "Freelance Angular developer", "joe.el@example.com", "+41 62 987 65 43", PersonType.Freelancer,
        addresses,
        TestUsers.Admin.Id, TestData.CreatedDate, TestUsers.Admin.Id, TestData.ModifiedDate, person.Version);

    private static UpdateAddressRequest ToUpdateAddressRequest(GetAddressResponse address) => new(
        address.Id, address.Street, address.Number, address.City, address.PostalCode,
        address.CreatedId, address.CreatedDate, TestUsers.Admin.Id, TestData.ModifiedDate, address.Version);
}
