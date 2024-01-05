using System.Reflection;
using FluentValidation.AspNetCore;
using JobLink.API.Helpers;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using JobLink.Business;
using JobLink.Business.Services.Implements;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.ExternalServices.Implements;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
});

builder.Services.AddFluentValidation(fln => {
    fln.RegisterValidatorsFromAssemblyContaining<AppUserService>();
});


builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>()
.AddSignInManager<SignInManager<AppUser>>()
.AddDefaultTokenProviders();


var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

builder.Services.AddAutoMapper(typeof(RegisterDto).Assembly);

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();

