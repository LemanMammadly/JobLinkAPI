using FluentValidation;

namespace JobLink.Business.Dtos.AppUserDtos;

public record LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginDtoValidator:AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(a => a.UserName)
            .NotEmpty().WithMessage("UserName field is not empty")
            .NotNull().WithMessage("UserName field is not null")
            .MinimumLength(2).WithMessage("Username minimum length must be 2");
        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Password field is not empty")
            .NotNull().WithMessage("Password field is not null")
            .MinimumLength(6).WithMessage("Password length must be equal or great 6");
    }
}
