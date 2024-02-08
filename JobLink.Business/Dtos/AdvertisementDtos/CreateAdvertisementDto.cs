using FluentValidation;
using JobLink.Core.Entities;
using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record CreateAdvertisementDto
{
    public string Title { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string WorkGraphic { get; set; }
    public string? Ability { get; set; }
    public string JobDesc { get; set; }
    public string Reqruiment { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public int CategoryId { get; set; }
}


public class CreateAdvertisementDtoValidator:AbstractValidator<CreateAdvertisementDto>
{
    public CreateAdvertisementDtoValidator()
    {
        RuleFor(a => a.Title)
            .MinimumLength(5).WithMessage("Advertisement title minimum length 5")
            .NotEmpty().WithMessage("Advertisement title not empty")
            .NotNull().WithMessage("Advertisement title not null");
        RuleFor(a => a.City)
            .NotEmpty().WithMessage("Advertisement city not empty")
            .NotNull().WithMessage("Advertisement city not null");
        RuleFor(a => a.Salary)
            .GreaterThan(344) .WithMessage("Salary minimum limit 345")
            .When(a => a.Salary != null);
        RuleFor(a => a.WorkGraphic)
            .NotEmpty().WithMessage("Advertisement workGraphic not empty")
            .NotNull().WithMessage("Advertisement workGraphic not null");
        RuleFor(a => a.Ability)
            .MinimumLength(20).WithMessage("Advertisement ability minimum length 20")
            .When(a => a.Ability != null);
        RuleFor(a => a.JobDesc)
            .MinimumLength(20).WithMessage("Advertisement job description minimum length 20")
            .NotEmpty().WithMessage("Advertisement job description not empty")
            .NotNull().WithMessage("Advertisement job description not null");
        RuleFor(a => a.Reqruiment)
            .MinimumLength(20).WithMessage("Advertisement reqruiment minimum length 20")
            .NotEmpty().WithMessage("Advertisement reqruiment not empty")
            .NotNull().WithMessage("Advertisement reqruiment not null");
        RuleFor(a => a.Experience)
            .MinimumLength(20).WithMessage("Advertisement expreience minimum length 20")
            .When(a => a.Experience != null);
        RuleFor(a => a.Education)
            .MinimumLength(20).WithMessage("Advertisement education minimum length 20")
            .When(a => a.Education != null);
        RuleFor(a => a.CategoryId)
            .NotEmpty().WithMessage("Advertisement category id not empty")
            .NotNull().WithMessage("Advertisement category id not null")
            .GreaterThan(0).WithMessage("Advertisement category id must be greather than 0");
    }
}