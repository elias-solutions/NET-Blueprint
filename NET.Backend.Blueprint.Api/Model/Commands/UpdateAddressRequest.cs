using System.ComponentModel.DataAnnotations;

namespace NET.Backend.Blueprint.Api.Model.Commands;

public record UpdateAddressRequest(
    [Required] Guid AddressId,
    [Required] string Street,
    [Required] string Number,
    [Required] string City,
    [Required] string PostalCode,
    [Required] Guid CreatedId,
    [Required] DateTimeOffset CreatedDate,
    [Required] Guid ModifiedId,
    [Required] DateTimeOffset ModifiedDate,
    [Required] Guid Version);
