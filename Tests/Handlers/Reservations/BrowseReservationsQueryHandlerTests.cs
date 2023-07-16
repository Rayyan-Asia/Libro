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
using Xunit;
using Application.Entities.Reservations.Handlers;
using Application;
using Moq;
using Domain;
using System.Reflection.Metadata;

namespace Tests.Handlers.Reservations
{
    public class BrowseReservationsQueryHandlerTests
    {
        private readonly Mock<IReservationRepository> _reservationRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BrowseReservationsQueryHandler>> _loggerMock;
        private readonly BrowseReservationsQueryHandler _handler;

        public BrowseReservationsQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _reservationRepositoryMock = new Mock<IReservationRepository>();
            _loggerMock = new Mock<ILogger<BrowseReservationsQueryHandler>>();
            _handler = new BrowseReservationsQueryHandler(
                _reservationRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task ShouldReturnAllReservations()
        {
            var pageNumber = 1;
            var pageSize = 10;

            var reservationsDtos = new List<ReservationDto>
            {
                new ReservationDto
                {
                    Id = 1,
                    UserId  = 1,
                    BookId =1 ,
                },
                new ReservationDto
                {
                    Id = 2,
                    UserId  = 2,
                    BookId =2 ,
                },
                new ReservationDto
                {
                    Id = 3,
                    UserId  = 3,
                    BookId =3 ,
                },
            };

            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = 1,
                    UserId  = 1,
                    BookId =1 ,
                },
                new Reservation
                {
                    Id = 2,
                    UserId  = 2,
                    BookId =2 ,
                },
                new Reservation
                {
                    Id = 3,
                    UserId  = 3,
                    BookId =3 ,
                },
            };


            var paginationMetadata = new PaginationMetadata
            {
                ItemCount = 3,
                PageSize = 10,
                PageCount = 1,
            };

            _reservationRepositoryMock.Setup(x => x.GetAllReservationsAsync(pageNumber, pageSize))
                .ReturnsAsync((paginationMetadata, reservations));
            _mapperMock.Setup(m => m.Map<List<ReservationDto>>(reservations)).Returns(reservationsDtos);

            var query = new BrowseReservationsQuery { pageNumber = pageNumber, pageSize = pageSize };
            var result = await _handler.Handle(query , default);

            Assert.Equal(paginationMetadata, result.Item1);
            Assert.Equal(reservationsDtos, result.Item2);
        }
    }
}