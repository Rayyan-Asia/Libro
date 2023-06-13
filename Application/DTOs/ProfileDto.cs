using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain;

namespace Application.DTOs
{
    public class ProfileDto 
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Role Role { get; set; } = Role.Patron;
        public List<Loan> Loans { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
