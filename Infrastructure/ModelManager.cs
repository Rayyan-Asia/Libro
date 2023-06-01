using System.Numerics;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Libro.Infrastructure
{
    public class ModelManager
    {
        private readonly ModelBuilder _modelBuilder;

        public ModelManager(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }


        public void ConfigureEntityProperties()
        {
            ConfigureUserProperties();
            ConfigureAuthorProperties();
            ConfigureGenreProperties();
            ConfigureBookProperties();
            ConfigureReservationProperties();
            ConfigureLoanProperties();
        }


        private void ConfigureUserProperties()
        {
            _modelBuilder.Entity<User>().Property<Role>(d => d.Role).IsRequired();
            _modelBuilder.Entity<User>().Property<string>(d => d.Name).HasMaxLength(32).IsRequired();
            _modelBuilder.Entity<User>().Property(d => d.PhoneNumber).HasMaxLength(16).IsRequired();
            _modelBuilder.Entity<User>().Property(d => d.Email).HasMaxLength(100).IsRequired();
        }
        private void ConfigureAuthorProperties()
        {
            _modelBuilder.Entity<Author>().Property<string>(d => d.Description).HasMaxLength(500);
            _modelBuilder.Entity<Author>().Property<string>(d => d.Name).HasMaxLength(32).IsRequired();
        }

        private void ConfigureGenreProperties()
        {
            _modelBuilder.Entity<Genre>().Property<string>(d => d.Type).HasMaxLength(32);
        }
        private void ConfigureBookProperties()
        {
            _modelBuilder.Entity<Book>().Property<string>(d => d.Description).HasMaxLength(500);
            _modelBuilder.Entity<Book>().Property<string>(d => d.Title).HasMaxLength(100).IsRequired();
            _modelBuilder.Entity<Book>().Property<bool>(d => d.IsReserved).HasDefaultValue(false);
            _modelBuilder.Entity<Book>().Property<int>(d => d.AuthorId).IsRequired();
        }

        private void ConfigureReservationProperties()
        {
            _modelBuilder.Entity<Reservation>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<Reservation>().Property<int>(d => d.BookId).IsRequired();
            _modelBuilder.Entity<Reservation>().Property<DateTime>(d => d.ReservationDate).HasColumnType("Date").IsRequired();
        }

        private void ConfigureLoanProperties()
        {
            _modelBuilder.Entity<Loan>().Property<int>(d => d.UserId).IsRequired();
            _modelBuilder.Entity<Loan>().Property<int>(d => d.BookId).IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime>(d => d.LoanDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime>(d => d.DueDate).HasColumnType("Date").IsRequired();
            _modelBuilder.Entity<Loan>().Property<DateTime?>(d => d.ReturnDate).HasColumnType("Date");
        }


    }
}