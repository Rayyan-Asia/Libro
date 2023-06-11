using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Books.Queries;
using Application.DTOs;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Books.Handlers
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, (PaginationMetadata, List<BookDto>)>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;

        public SearchQueryHandler(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(PaginationMetadata, List<BookDto>)> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, books) = await _repository.SearchBooksAsync(request.Title,request.Author,request.Genre,request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
