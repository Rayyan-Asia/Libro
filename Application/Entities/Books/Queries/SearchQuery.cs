using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Entities.Books.Queries
{
    public class SearchQuery : IRequest<(PaginationMetadata, List<BookDto>)>
    {
        [MaxLength(100)]
        public string Title { get; set; }
        
        [MaxLength(32)]
        public string Author { get; set; }

        [MaxLength(32)]
        public string Genre { get; set; }

        [Range(0, 5)]
        public int pageSize { get; set; } = 5;
        public int pageNumber { get; set; } = 0;

    }
}
