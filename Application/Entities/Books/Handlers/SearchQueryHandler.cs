using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Queries;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, (PaginationMetadata, List<BookDto>)>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchQueryHandler> _logger;
        public SearchQueryHandler(IBookRepository repository, IMapper mapper, ILogger<SearchQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<BookDto>)> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching Books");
            var (paginationMetadata, books) = await _repository.SearchBooksAsync(request.Title, request.Author, request.Genre, request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
