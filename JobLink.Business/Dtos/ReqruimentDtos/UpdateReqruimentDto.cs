using FluentValidation;

namespace JobLink.Business.Dtos.ReqruimentDtos;

public record UpdateReqruimentDto
{
    public string Text { get; set; }
}


public class UpdateReqruimentDtoValidator : AbstractValidator<UpdateReqruimentDto>
{
    public UpdateReqruimentDtoValidator()
    {
        RuleFor(r => r.Text)
            .NotEmpty().WithMessage("Job reqruiment cannot be empty")
            .NotNull().WithMessage("Job reqruiment  cannot be null")
            .MinimumLength(2).WithMessage("Job reqruiment length must be greater than 2");
    }
}