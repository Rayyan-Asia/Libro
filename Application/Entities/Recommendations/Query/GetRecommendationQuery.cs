using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Recommendations.Query
{
    public class GetRecommendationQuery : IRequest<BookDto>
    {
        public int UserId;
    }
}
