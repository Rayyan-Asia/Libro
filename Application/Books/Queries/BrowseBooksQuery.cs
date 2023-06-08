using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Infrastructure;
using MediatR;

namespace Application.Books.Queries
{
    public class BrowseBooksQuery : IRequest<(PaginationMetadata, List<BookDto>)>
    {
        public int pageSize { get; set; } = 5;
        public int pageNumber { get; set; } = 0;
    }
}
