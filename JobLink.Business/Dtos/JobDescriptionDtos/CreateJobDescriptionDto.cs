using FluentValidation;

namespace JobLink.Business.Dtos.JobDescriptionDtos;

public record CreateJobDescriptionDto
{
    public string Description { get; set; }
}

public class CreateJobDescriptionDtoValidator:AbstractValidator<CreateJobDescriptionDto>
{
    public CreateJobDescriptionDtoValidator()
    {
        RuleFor(jd=>jd.Description)
            .NotEmpty().WithMessage("Job description cannot be empty")
            .NotNull().WithMessage("Job description  cannot be null")
            .MinimumLength(2).WithMessage("Job description length must be greater than 2");
    }
}