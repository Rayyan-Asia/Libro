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

namespace Application.Entities.Authors.Handlers
{
    public class BrowseAuthorsQueryHandler : IRequestHandler<BrowseAuthorsQuery, (PaginationMetadata, List<AuthorDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repository;

        public BrowseAuthorsQueryHandler(IMapper mapper, IAuthorRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<(PaginationMetadata, List<AuthorDto>)> Handle(BrowseAuthorsQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, authors) = await _repository.GetAuthorsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<AuthorDto>>(authors);
            return (paginationMetadata, dtoList);
        }
    }
}
