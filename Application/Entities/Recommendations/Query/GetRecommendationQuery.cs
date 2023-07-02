using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Recommendations.Query
{
    public class GetRecommendationQuery : IRequest<IActionResult>
    {
        public int UserId;
    }
}
