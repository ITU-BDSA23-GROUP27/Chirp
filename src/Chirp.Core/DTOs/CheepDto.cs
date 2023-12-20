using FluentValidation;

namespace Chirp.Core.DTOs;

/// <summary>
/// Data Transfer Object (DTO) for Cheeps in the Chirp application.
/// A Cheep is a representation of a post in the Chirp application.
/// Cheeps are used for users to post messages and to display messages on the timelines.
/// </summary>

public class CheepDto
{
    public Guid Id { get; set; }
    public required string Message { get; init; }
    public required string TimeStamp { get; init; }
    public required string UserName { get; init; }
}

public class CheepValidator : AbstractValidator<CheepDto>
{
    public CheepValidator()
    {
        RuleFor(c => c.Message).NotEmpty().WithMessage("Please write a cheep containing at least one character.");
        RuleFor(c => c.Message).Length(0, 160);
    }
}