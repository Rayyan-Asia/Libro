using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Authors.Queries;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Authors.Handlers
{
    public class BrowseAuthorsQueryHandler : IRequestHandler<BrowseAuthorsQuery, (PaginationMetadata, List<AuthorDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repository;
        private readonly ILogger<BrowseAuthorsQueryHandler> _logger;

        public BrowseAuthorsQueryHandler(IMapper mapper, IAuthorRepository repository, ILogger<BrowseAuthorsQueryHandler> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<AuthorDto>)> Handle(BrowseAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Authors from database");
            var (paginationMetadata, authors) = await _repository.GetAuthorsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<AuthorDto>>(authors);
            return (paginationMetadata, dtoList);
        }
    }
}
