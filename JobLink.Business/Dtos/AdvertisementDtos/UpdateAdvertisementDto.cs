using FluentValidation;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record UpdateAdvertisementDto
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
    public int? CategoryId { get; set; }
}


public class UpdateAdvertisementDtoValidator:AbstractValidator<UpdateAdvertisementDto>
{
    public UpdateAdvertisementDtoValidator()
    {
        RuleFor(a => a.Title)
            .NotEmpty().WithMessage("Advertisement title not empty")
            .NotNull().WithMessage("Advertisement title not null");
        RuleFor(a => a.City)
            .NotEmpty().WithMessage("Advertisement city not empty")
            .NotNull().WithMessage("Advertisement city not null");
        RuleFor(a => a.Salary)
            .GreaterThan(345).WithMessage("Salary minimum limit 345")
            .When(a => a.Salary != null);
        RuleFor(a => a.WorkGraphic)
            .NotEmpty().WithMessage("Advertisement workGraphic not empty")
            .NotNull().WithMessage("Advertisement workGraphic not null");
        RuleFor(a => a.Ability)
            .MinimumLength(20).WithMessage("Advertisement ability minimum length 20")
            .When(a => a.Ability != null);
        RuleFor(a => a.JobDesc)
            .NotEmpty().WithMessage("Advertisement job description not empty")
            .NotNull().WithMessage("Advertisement job description not null");
        RuleFor(a => a.Reqruiment)
            .NotEmpty().WithMessage("Advertisement reqruiment not empty")
            .NotNull().WithMessage("Advertisement reqruiment not null");
        RuleFor(a => a.Experience)
            .MinimumLength(20).WithMessage("Advertisement expreience minimum length 20")
            .When(a => a.Experience != null);
        RuleFor(a => a.Education)
            .MinimumLength(20).WithMessage("Advertisement education minimum length 20")
            .When(a => a.Education != null);
        RuleFor(a => a.CategoryId)
            .GreaterThan(0).WithMessage("Advertisement category id must be greather than 0")
            .When(a => a.CategoryId != null);
    }
}
