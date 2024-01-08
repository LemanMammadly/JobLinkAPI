using System.Text.RegularExpressions;
using FluentValidation;

namespace JobLink.Business.Dtos.AppUserDtos;

public record UpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
}


public class UpdateDtoValidator:AbstractValidator<UpdateDto>
{
    public UpdateDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name field is not empty")
            .NotNull().WithMessage("Name field is not null")
            .MinimumLength(2).WithMessage("Name minimum length must be 2");
        RuleFor(r => r.Surname)
            .NotEmpty().WithMessage("Surname field is not empty")
            .NotNull().WithMessage("Surname field is not null")
            .MinimumLength(2).WithMessage("Surname minimum length must be 2");
        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage("Username field is not empty")
            .NotNull().WithMessage("Username field is not null")
            .MinimumLength(2).WithMessage("Username minimum length must be 2");
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email field is not empty")
            .NotNull().WithMessage("Email field is not null")
            .Must(u =>
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var result = regex.Match(u);
                return result.Success;
            }).WithMessage("Please enter valid email");
    }
}
