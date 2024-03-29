﻿using FluentValidation;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Core.Entities;
using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record CreateAdvertisementDto
{
    public string Title { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string WorkGraphic { get; set; }
    public List<int>? AbilityIds { get; set; }
    public List<string> Desc { get; set; }
    public List<string> Reqruiment { get; set; }
    public string? Experience { get; set; }
    public Education? Education { get; set; }
    public int CategoryId { get; set; }
}


public class CreateAdvertisementDtoValidator:AbstractValidator<CreateAdvertisementDto>
{
    public CreateAdvertisementDtoValidator()
    {
        RuleFor(a => a.Title)
            .MinimumLength(5).WithMessage("Advertisement title minimum length 5")
            .NotEmpty().WithMessage("Advertisement title not empty")
            .NotNull().WithMessage("Advertisement title not null");
        RuleFor(a => a.City)
            .NotEmpty().WithMessage("Advertisement city not empty")
            .NotNull().WithMessage("Advertisement city not null");
        RuleFor(a => a.Salary)
            .GreaterThan(344) .WithMessage("Salary minimum limit 345")
            .When(a => a.Salary != null);
        RuleFor(a => a.WorkGraphic)
            .NotEmpty().WithMessage("Advertisement workGraphic not empty")
            .NotNull().WithMessage("Advertisement workGraphic not null");
        RuleFor(a => a.AbilityIds)
            .Must(a => CheckSameId(a)).WithMessage("You cannot add same ability id")
            .When(a => a.AbilityIds != null);
        RuleFor(a => a.Experience)
            .MinimumLength(20).WithMessage("Advertisement expreience minimum length 20")
            .When(a => a.Experience != null);
        RuleFor(a => a.Education)
            .Must(BeAValidEducation)
            .WithMessage("Invalid education")
            .When(a => a.Education != null);
        RuleFor(a => a.CategoryId)
            .NotEmpty().WithMessage("Advertisement category id not empty")
            .NotNull().WithMessage("Advertisement category id not null")
            .GreaterThan(0).WithMessage("Advertisement category id must be greather than 0");
    }

    private bool BeAValidEducation(Education? education)
    {
        return education.HasValue && Enum.IsDefined(typeof(Education), education.Value);
    }

    private bool CheckSameId(List<int> ids)
    {
        var encounteredIds = new HashSet<int>();

        if (ids == null || ids.Count == 0)
        {
            return false;
        }

        foreach (var id in ids)
        {
            if (!encounteredIds.Contains(id))
            {
                encounteredIds.Add(id);
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}