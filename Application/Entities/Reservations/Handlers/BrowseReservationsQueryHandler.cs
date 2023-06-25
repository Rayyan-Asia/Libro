using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Books.Queries;
using Application.Entities.Reservations.Queries;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Reservations.Handlers
{
    public class BrowseReservationsQueryHandler : IRequestHandler<BrowseReservationsQuery, (PaginationMetadata, List<ReservationDto>)>
    {
        private readonly IReservationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BrowseReservationsQueryHandler> _logger;

        public BrowseReservationsQueryHandler(IReservationRepository repository, IMapper mapper, ILogger<BrowseReservationsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<(PaginationMetadata, List<ReservationDto>)> Handle(BrowseReservationsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving active reservations");
            var (paginationMetadata, books) = await _repository.GetAllReservationsAsync(request.pageNumber, request.pageSize);
            var dtoList = _mapper.Map<List<ReservationDto>>(books);
            return (paginationMetadata, dtoList);
        }
    }
}
