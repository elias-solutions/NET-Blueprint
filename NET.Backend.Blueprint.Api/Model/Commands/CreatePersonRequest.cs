using System.ComponentModel.DataAnnotations;

namespace NET.Backend.Blueprint.Api.Model.Commands;

public record CreatePersonRequest(
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateOnly Birthday,
    [Required] Guid CreatedId,
    [Required] DateTimeOffset CreatedDate,
    [Required] CreateAddressRequest[] Addresses);