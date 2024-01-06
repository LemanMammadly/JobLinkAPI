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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        LifetimeValidator = (_, expires, token, _) => token != null ? DateTime.UtcNow.AddHours(4) < expires : false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
});
builder.Services.AddAuthorization();

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

