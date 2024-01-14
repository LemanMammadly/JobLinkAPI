﻿using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Validators;

public class FileValidator:AbstractValidator<IFormFile>
{
    public FileValidator()
    {
    }

    public FileValidator(int sizeWithMb=3,string contentType="image")
    {
        RuleFor(f => f.ContentType)
            .Must(t => t.Contains(contentType)).WithMessage("File format is wrong");
        RuleFor(f => f.Length)
            .LessThanOrEqualTo(sizeWithMb *1024 * 1024) .WithMessage("File max size must be " + sizeWithMb + "mb");
    }
}

