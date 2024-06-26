﻿using System.Reflection;
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
using Microsoft.OpenApi.Models;
using JobLink.Business.Constants;
using JobLink.DAL;
using Hangfire;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.


builder.Services.AddControllers().AddNewtonsoftJson(opt =>
    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


//Hangfire

var hangfireConnectionString = builder.Configuration["ConnectionStrings:Hangfire"];
builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(hangfireConnectionString);
    RecurringJob.AddOrUpdate<AdvertisementService>(a => a.CheckStatus(), "0 0 * * *");
    RecurringJob.AddOrUpdate<AdvertisementService>(a => a.ExpiredDeletion(), "0 0 * * *");
});

builder.Services.AddHangfireServer();


//Cors

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();

        });
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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
builder.Services.AddRepositories();

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

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
    });
}



app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot/imgs")),
    RequestPath = "/wwwroot/imgs"
});

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard();

//app.UseCustomExceptionHandler();

app.MapControllers();

RootConstant.Root = builder.Environment.WebRootPath;

app.Run();

