using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Returns.Queries
{
    public class BrowseReturnsQuery : IRequest<(PaginationMetadata, List<BookReturnDto>)>
    {
        [Range(0, 5)]
        public int pageSize { get; set; } = 5;
        public int pageNumber { get; set; } = 0;
    }
}
