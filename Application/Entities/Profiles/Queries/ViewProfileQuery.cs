using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Profiles.Queries
{
    public class ViewProfileQuery : IRequest<IActionResult>
    {
        public int PatronId { get; set; }
    }
}
