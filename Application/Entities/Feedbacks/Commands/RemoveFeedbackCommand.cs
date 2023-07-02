using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Feedbacks.Commands
{
    public class RemoveFeedbackCommand : IRequest<IActionResult>
    {
        [Required]
        public int FeedbackId { get; set; }
        
        [Required]
        public int UserId { get; set; }
    }
}
