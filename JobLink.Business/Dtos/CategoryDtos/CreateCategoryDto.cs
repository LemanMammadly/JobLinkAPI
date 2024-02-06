using FluentValidation;
using JobLink.Business.Validators;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Dtos.CategoryDtos;

public record CreateCategoryDto
{
    public string Name { get; set; }
    public IFormFile LogoFile { get; set; }
}

public class CreateCategoryDtoValidator:AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name cannot be empty")
            .NotNull().WithMessage("Category name cannot be null")
            .MinimumLength(2).WithMessage("Category name length must be great than 2");
        RuleFor(c => c.LogoFile)
            .NotEmpty().WithMessage("Category logo cannot be empty")
            .NotNull().WithMessage("Category logo cannot be null")
            .SetValidator(new FileValidator());
    }
}