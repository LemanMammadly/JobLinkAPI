using System.Data;
using System.Security.Cryptography;
using System.Xml.Linq;
using FluentValidation;

namespace JobLink.Business.Dtos.RolesDtos;

public record UpdateRolesDto
{
    public string RoleName { get; set; }
    public string NewRoleName { get; set; }
}


public class UpdateRolesDtoValidator:AbstractValidator<UpdateRolesDto>
{
    public UpdateRolesDtoValidator()
    {
        RuleFor(r => r.RoleName)
        .NotEmpty().WithMessage("Old role name is not be empty")
            .NotNull().WithMessage("Old role name is not be null");
        RuleFor(r => r.NewRoleName)
            .NotEmpty().WithMessage("Old role name is not be empty")
            .NotNull().WithMessage("Old role name is not be null");
    }
}