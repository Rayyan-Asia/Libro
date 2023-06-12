using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Application.Profiles;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Libro.Infrastructure;
using MediatR;
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



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LibroDbContext>(DbContextOptions => DbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:LibroDbConnectionString"]));

            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<ILoanRepository, LoanRepository>();

            builder.Services.AddScoped<UserValidator>();

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddAutoMapper(
                typeof(Program).GetTypeInfo().Assembly,
                typeof(UserProfile).GetTypeInfo().Assembly,
                typeof(BookProfile).GetTypeInfo().Assembly);
            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


            builder.Services.AddAuthentication("Bearer")
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