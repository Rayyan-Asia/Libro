using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Users;
using Application.Users.Commands;
using Application.Users.Handlers;
using Application.Users.Queries;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Libro.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Validators;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddDbContext<LibroDbContext>(DbContextOptions => DbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:LibroDbConnectionString"]));

            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<UserValidator>();
            builder.Services.AddScoped<IRequestHandler<LoginQuery, AuthenticationResponse>, LoginQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<RegisterCommand, AuthenticationResponse>, RegisterCommandHandler>();


            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.IncludeErrorDetails = true;

                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateIssuerSigningKey = true,
                      ValidateLifetime = true,
                      ValidIssuer = builder.Configuration["Authentication:Issuer"],
                      ValidAudience = builder.Configuration["Authentication:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"])),
                  };
              });

            

            builder.Services.AddMvc(); 

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorRequired", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Administrator");
                });

                options.AddPolicy("LibrarianRequired", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Librarian");
                });

                options.AddPolicy("PatronRequired", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Patron");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.MapControllers();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.Run();
        }
    }
}