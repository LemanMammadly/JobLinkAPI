using FluentValidation;

namespace JobLink.Business.Dtos.AppUserDtos;

public record AddRoleDto
{
    public string UserId { get; set; }
    public string RoleName { get; set; }
}

public class AddRoleDtoValidator:AbstractValidator<AddRoleDto>
{
    public AddRoleDtoValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull().WithMessage("UserId could not be null")
            .NotEmpty().WithMessage("UserId could not be empty");
        RuleFor(r => r.RoleName)
            .NotNull().WithMessage("RoleName could not be null")
            .NotEmpty().WithMessage("RoleName could not be empty");
    }
}