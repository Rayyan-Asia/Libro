using System.Reflection;
using System.Text;
using Application.Interfaces;
using Application.Profiles;
using AutoDependencyRegistration;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using Libro.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Presentation.Validators.Users;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Hangfire;
using Hangfire.SqlServer;

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

            builder.Services.AddValidatorsFromAssemblyContaining<LoginQueryValidator>();

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

            builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDbConnectionString")));

            // Add the processing server as IHostedService
            builder.Services.AddHangfireServer();



            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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
            app.UseHangfireDashboard();
            app.MapHangfireDashboard();

            app.Run();
        }
    }
}