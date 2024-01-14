using FluentValidation;
using JobLink.Business.Validators;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Dtos.IndustryDtos;

public record CreateIndustryDto
{
    public string Name { get; set; }
    public IFormFile LogoFile { get; set; }
}

public class CreateIndustryDtoValidator:AbstractValidator<CreateIndustryDto>
{
    public CreateIndustryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Industry name cannot be empty")
            .NotNull().WithMessage("Industry name cannot be null")
            .MinimumLength(2).WithMessage("Industry name length must be great than 2");
        RuleFor(c => c.LogoFile)
            .NotEmpty().WithMessage("Industry logo cannot be empty")
            .NotNull().WithMessage("Industry logo cannot be null")
            .SetValidator(new FileValidator());
    }
}

