using NET.Backend.Blueprint.Application.Model.Commands;
using NET.Backend.Blueprint.Domain.Enums;

namespace NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests.Environment;

public static class TestData
{
    public static readonly DateTimeOffset CreatedDate = DateTimeOffset.Parse("2023-10-10T14:30:00+00:00");
    public static readonly DateTimeOffset ModifiedDate = DateTimeOffset.Parse("2023-11-10T14:30:00+00:00");

    public static CreatePersonRequest CreatePersonRequest() => new(
        "Jonas",
        "Elias",
        new DateOnly(1985, 4, 12),
        Gender.Male,
        "Founder of Elias Solutions",
        "jonas.elias@example.com",
        "+41 62 000 00 00",
        PersonType.Employee,
        TestUsers.Admin.Id,
        CreatedDate,
        [
            new CreateAddressRequest("Solothurnerstrasse", "2", "Hägendorf", "4614", TestUsers.Admin.Id, CreatedDate),
            new CreateAddressRequest("Bahnhofstrasse", "1", "Olten", "4600", TestUsers.Admin.Id, CreatedDate)
        ]);
}
