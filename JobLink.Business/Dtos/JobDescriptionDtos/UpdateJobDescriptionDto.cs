using FluentValidation;

namespace JobLink.Business.Dtos.JobDescriptionDtos;

public record UpdateJobDescriptionDto
{
    public string Description { get; set; }
}

public class UpdateJobDescriptionDtoValidator : AbstractValidator<CreateJobDescriptionDto>
{
    public UpdateJobDescriptionDtoValidator()
    {
        RuleFor(jd => jd.Description)
            .NotEmpty().WithMessage("Job description cannot be empty")
            .NotNull().WithMessage("Job description  cannot be null")
            .MinimumLength(2).WithMessage("Job description length must be great than 2");
    }
}

