using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Reservations.Queries;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Application.Entities.Returns.Queries;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Returns.Handlers
{
    public class BrowseReturnsQueryHandler : IRequestHandler<BrowseReturnsQuery, (PaginationMetadata, List<BookReturnDto>)>
    {
        private readonly IBookReturnRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrowseReturnsQueryHandler> _logger;

        public BrowseReturnsQueryHandler(IBookReturnRepository repository, IMapper mapper, ILogger<BrowseReturnsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<BookReturnDto>)> Handle(BrowseReturnsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book returns");
            var (paginationMetadata, returns) = await _repository.GetAllReturnsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookReturnDto>>(returns);
            return (paginationMetadata, dtoList);
        }
    }
}
