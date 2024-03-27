using FluentValidation;

namespace JobLink.Business.Dtos.ReqruimentDtos;

public record CreateReqruimentDto
{
    public string Text { get; set; }
}

public class CreateReqruimentDtoValidator:AbstractValidator<CreateReqruimentDto>
{
    public CreateReqruimentDtoValidator()
    {
        RuleFor(r=>r.Text)
            .NotEmpty().WithMessage("Job reqruiment cannot be empty")
            .NotNull().WithMessage("Job reqruiment  cannot be null")
            .MinimumLength(2).WithMessage("Job reqruiment length must be greater than 2");
    }
}
