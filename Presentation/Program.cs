using Application.Interfaces;
using Application.Services;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<UserValidator>();

            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}