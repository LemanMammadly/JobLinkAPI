using FluentValidation;

namespace JobLink.Business.Dtos.AppUserDtos;

public record ChangePasswordDto
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ChangePasswordDtoValidator:AbstractValidator<ChangePasswordDto>
{
    public ChangePasswordDtoValidator()
    {
        RuleFor(c=>c.OldPassword)
            .NotEmpty().WithMessage("Password field is not empty")
            .NotNull().WithMessage("Password field is not null")
            .MinimumLength(6).WithMessage("Password length must be equal or great 6");
        RuleFor(c => c.NewPassword)
            .NotEmpty().WithMessage("Password field is not empty")
            .NotNull().WithMessage("Password field is not null")
            .MinimumLength(6).WithMessage("Password length must be equal or great 6");
        RuleFor(r => r.ConfirmPassword)
            .Equal(r => r.NewPassword).WithMessage("Pasword must be same ConfirmPassword");
    }
}


