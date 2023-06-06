using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Application.DTOs;

namespace Application.Users.Commands
{
    public class RegisterCommand : IRequest<(UserDto,string)>
    {
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
        public string Password { get; set; }
    }
}
