using System.Text.RegularExpressions;
using FluentValidation;
using JobLink.Business.Validators;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Dtos.CompanyDtos;

public record CreateCompanyDto
{
    public string Name { get; set; }
    public IFormFile? LogoFile { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string? Website { get; set; }
    public List<int> IndustryIds { get; set; }
}


public class CreateCompanyDtoValidator:AbstractValidator<CreateCompanyDto>
{
    public CreateCompanyDtoValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Company name cannot be empty")
            .NotNull().WithMessage("Company name cannot be null")
            .MinimumLength(2).WithMessage("Company name length must be great than 2");
        RuleFor(c => c.LogoFile)
            .SetValidator(new FileValidator());
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Company Description cannot be empty")
            .NotNull().WithMessage("Company Description cannot be null");
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email field is not empty")
            .NotNull().WithMessage("Email field is not null")
            .Must(u =>
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                var result = regex.Match(u);
                return result.Success;
            }).WithMessage("Please enter valid email");
        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Company phone cannot be empty")
            .NotNull().WithMessage("Company phone cannot be null");
        RuleFor(c => c.Address)
            .NotEmpty().WithMessage("Company Address cannot be empty")
            .NotNull().WithMessage("Company Address cannot be null");
        RuleFor(c => c.Website)
            .Must(u =>
            {
                Regex regex = new Regex("^((ftp|http|https):\\/\\/)?(www.)?(?!.*(ftp|http|https|www.))[a-zA-Z0-9_-]+(\\.[a-zA-Z]+)+((\\/)[\\w#]+)*(\\/\\w+\\?[a-zA-Z0-9_]+=\\w+(&[a-zA-Z0-9_]+=\\w+)*)?\\/?$");
                var result = regex.Match(u);
                return result.Success;
            }).WithMessage("Please enter valid website")
            .When(c => c.Website != null);
        RuleFor(c => c.IndustryIds)
            .NotEmpty().WithMessage("IndustryIds cannot be empty")
            .NotNull().WithMessage("IndustryIds cannot be null")
            .Must(c => CheckIds(c)).WithMessage("You cannot add same industry id");
    }

    private bool CheckIds(List<int> ids)
    {
        var encounteredIds = new HashSet<int>();

        if (ids == null || ids.Count == 0)
        {
            return false;
        }

        foreach (var id in ids)
        {
            if(!encounteredIds.Contains(id))
            {
                encounteredIds.Add(id);
            }
            else {
                return false;
            }
        }

        return true;
    }
}
