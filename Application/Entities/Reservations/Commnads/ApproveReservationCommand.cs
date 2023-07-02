using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Reservations.Commnads
{
    public class ApproveReservationCommand : IRequest<IActionResult>
    {
        public int ReservationId { get; set; }
    }
}
