using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Reservations.Queries;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure;
using MediatR;
using Application.Entities.Returns.Queries;

namespace Application.Entities.Returns.Handlers
{
    public class BrowseReturnsQueryHandler : IRequestHandler<BrowseReturnsQuery, (PaginationMetadata, List<BookReturnDto>)>
    {
        private readonly IBookReturnRepository _repository;
        private readonly IMapper _mapper;

        public BrowseReturnsQueryHandler(IBookReturnRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(PaginationMetadata, List<BookReturnDto>)> Handle(BrowseReturnsQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, returns) = await _repository.GetAllReturnsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<BookReturnDto>>(returns);
            return (paginationMetadata, dtoList);
        }
    }
}
