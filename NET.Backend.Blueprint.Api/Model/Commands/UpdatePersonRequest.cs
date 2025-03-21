using System.ComponentModel.DataAnnotations;

namespace NET.Backend.Blueprint.Api.Model.Commands;

public record UpdatePersonRequest(
    [Required] Guid Id,
    [Required] string FirstName,
    [Required] string LastName,
    [Required] DateOnly Birthday,
    [Required] IEnumerable<UpdateAddressRequest> Addresses,
    [Required] Guid CreatedId,
    [Required] DateTimeOffset CreatedDate,
    [Required] Guid ModifiedId,
    [Required] DateTimeOffset ModifiedDate,
    [Required] Guid Version);