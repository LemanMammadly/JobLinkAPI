using FluentValidation;

namespace JobLink.Business.Dtos.AppUserDtos;

public record RemoveRoleDto
{
    public string UserId { get; set; }
    public string RoleName { get; set; }
}


public class RemoveRoleDtoValidator:AbstractValidator<RemoveRoleDto>
{
    public RemoveRoleDtoValidator()
    {
        RuleFor(r => r.UserId)
            .NotNull().WithMessage("UserId could not be null")
            .NotEmpty().WithMessage("UserId could not be empty");
        RuleFor(r => r.RoleName)
            .NotNull().WithMessage("RoleName could not be null")
            .NotEmpty().WithMessage("RoleName could not be empty");
    }
}