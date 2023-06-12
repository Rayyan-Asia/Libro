using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain;
using MediatR;

namespace Application.Entities.Reservations.Commnads
{
    public class ApproveReservationCommand : IRequest<LoanDto>
    {
        public int ReservationId { get; set; }
    }
}
