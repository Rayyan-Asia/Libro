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
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class BrowseBooksQueryHandler : IRequestHandler<BrowseBooksQuery, (PaginationMetadata, List<BookDto>)>
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrowseBooksQueryHandler> _logger;

        public BrowseBooksQueryHandler(IBookRepository repository, IMapper mapper, ILogger<BrowseBooksQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<BookDto>)> Handle(BrowseBooksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving books");
            var (paginationMetadata, books) = await _repository.GetBooksAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
