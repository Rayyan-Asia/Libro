using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Feedbacks.Commands
{
    public class AddFeedbackCommand : IRequest<IActionResult>
    {
        [Required]
        public Rating Rating { get; set; }

        [Required]
        [MaxLength(500)]
        public string Review { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
