using System.ComponentModel.DataAnnotations;

namespace NET.Backend.Blueprint.Application.Model.Commands;

public record CreateAddressRequest(
    [Required] string Street,
    [Required] string Number,
    [Required] string City,
    [Required] string PostalCode,
    [Required] Guid CreatedId,
    [Required] DateTimeOffset CreatedDate);