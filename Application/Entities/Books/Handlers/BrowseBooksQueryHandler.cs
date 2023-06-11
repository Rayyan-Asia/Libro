using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Queries;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Books.Handlers
{
    public class BrowseBooksQueryHandler : IRequestHandler<BrowseBooksQuery, (PaginationMetadata, List<BookDto>)>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public BrowseBooksQueryHandler(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(PaginationMetadata, List<BookDto>)> Handle(BrowseBooksQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, books) = await _repository.GetBooksAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
