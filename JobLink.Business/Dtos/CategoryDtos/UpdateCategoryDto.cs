using FluentValidation;
using JobLink.Business.Validators;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Dtos.CategoryDtos;

public record UpdateCategoryDto
{
    public string Name { get; set; }
    public IFormFile? LogoFile { get; set; }
}


public class UpdateCategoryDtoValidator:AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name cannot be empty")
            .NotNull().WithMessage("Category name cannot be null")
            .MinimumLength(2).WithMessage("Category name length must be great than 2");
        RuleFor(c => c.LogoFile)
            .SetValidator(new FileValidator());
    }
}
