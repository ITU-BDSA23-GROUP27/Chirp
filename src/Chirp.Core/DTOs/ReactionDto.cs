using FluentValidation;

namespace Chirp.Core.DTOs;

/// <summary>
/// Data Transfer Object (DTO) for handling reactions in the Chirp application.
/// </summary>

public class ReactionDto
{
    public Guid UserId { get; init; }
    public Guid CheepId { get; init; }
    public required string TimeStamp { get; init; }
    public string? Comment { get; init; }
}

public class ReactionValidator : AbstractValidator<ReactionDto>
{
    public ReactionValidator()
    {
        RuleFor(r => r.Comment).NotEmpty().WithMessage("Please write a comment containing at least one character.");
        RuleFor(r => r.Comment).Length(0, 160);
    }
}