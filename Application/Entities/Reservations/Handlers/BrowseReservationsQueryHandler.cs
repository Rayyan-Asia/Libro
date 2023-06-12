using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Queries;
using Application.Entities.Reservations.Queries;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Reservations.Handlers
{
    public class BrowseReservationsQueryHandler : IRequestHandler<BrowseReservationsQuery, (PaginationMetadata, List<ReservationDto>)>
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;

        public BrowseReservationsQueryHandler(IReservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<(PaginationMetadata, List<ReservationDto>)> Handle(BrowseReservationsQuery request, CancellationToken cancellationToken)
        {
            var (paginationMetadata, books) = await _repository.GetAllReservationsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<ReservationDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
