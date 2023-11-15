using FluentValidation;

namespace Chirp.Core.DTOs;

public class CheepDto
{
    
    // made to public records instead ?
    public Guid Id { get; set; }
    public required string Message { get; set; }
    public required string TimeStamp { get; set; }
    public required string AuthorName { get; set; }
}

public class CheepValidator : AbstractValidator<CheepDto>
{
    public CheepValidator()
    {
        RuleFor(c => c.Message).NotEmpty().WithMessage("Please write a cheep containing at least one character.");
        RuleFor(c => c.Message).Length(0, 20);
    }
}