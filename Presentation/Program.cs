using System.Reflection;
using System.Text;
using Application.Profiles;
using AutoDependencyRegistration;
using FluentValidation.AspNetCore;
using Libro.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LibroDbContext>(DbContextOptions => DbContextOptions.UseSqlServer(builder.Configuration["ConnectionStrings:LibroDbConnectionString"]));

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AutoRegisterDependencies();

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddXmlDataContractSerializerFormatters();

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

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
                options.AddPolicy("AdministratorOrLibrarianRequired", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Administrator", "Librarian");
                });
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