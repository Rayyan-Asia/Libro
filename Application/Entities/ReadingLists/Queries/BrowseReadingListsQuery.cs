using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.ReadingLists.Queries
{
    public class BrowseReadingListsQuery : IRequest<(PaginationMetadata, List<ReadingListDto>)>
    {
        [Range(0, 5)]
        public int pageSize { get; set; } = 5;
        public int pageNumber { get; set; } = 0;

        public int UserId { get; set; }
    }
}
