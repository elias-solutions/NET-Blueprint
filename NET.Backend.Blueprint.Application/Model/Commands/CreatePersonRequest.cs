using System.ComponentModel.DataAnnotations;
using NET.Backend.Blueprint.Domain.Entities;
using NET.Backend.Blueprint.Domain.Enums;

namespace NET.Backend.Blueprint.Application.Model.Commands;

public record CreatePersonRequest(
    [Required] string FirstName,
    [Required] string LastName,
    DateOnly? Birthday,
    Gender? Gender,
    [MaxLength(PersonConstraints.DescriptionMaxLength)] string? Description,
    [EmailAddress, MaxLength(PersonConstraints.EmailMaxLength)] string? Email,
    [MaxLength(PersonConstraints.PhoneMaxLength)] string? Phone,
    PersonType? Type,
    [Required] Guid CreatedId,
    [Required] DateTimeOffset CreatedDate,
    [Required] CreateAddressRequest[] Addresses);