using FluentValidation;

namespace Chirp.Core.DTOs;

public class CheepDto
{
    public Guid Id { get; set; }
    public required string Message { get; set; }
    public required string TimeStamp { get; set; }
    public required string UserName { get; set; }
    public bool IsCurrentUserFollowing { get; set; }

}

public class CheepValidator : AbstractValidator<CheepDto>
{
    public CheepValidator()
    {
        RuleFor(c => c.Message).NotEmpty().WithMessage("Please write a cheep containing at least one character.");
        RuleFor(c => c.Message).Length(0, 160);
    }
}