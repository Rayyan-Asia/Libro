﻿using System.Text.Json;
using Application.Entities.Feedbacks.Commands;
using Application.Entities.Reservations.Commnads;
using Application.Entities.Reservations.Queries;
using Application.Services;
using Domain;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validators.Reservations;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {

        private readonly IMediator _mediator;
        private readonly ReserveBookCommandValidator _reserveBookCommandValidator;
        private readonly ApproveReservationCommandValidator _approveReservationValidator;

        public ReservationsController(IMediator mediator, ReserveBookCommandValidator reserveBookCommandValidator,
            ApproveReservationCommandValidator approveReservationValidator)
        {
            _mediator = mediator;
            _reserveBookCommandValidator = reserveBookCommandValidator;
            _approveReservationValidator = approveReservationValidator;
        }

        [HttpPost("reserve/{bookId}")]
        [Authorize(Policy = "PatronRequired")]
        public async Task<IActionResult> Reserve(int bookId)
        {
            if (HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                if (authorizationHeader.Count > 0)
                {
                    var token = authorizationHeader[0]?.Split(" ")[1]; // Extract the JWT token
                    User user;
                    try
                    {
                        user = JwtService.GetUserFromPayload(token);
                    }
                    catch (Exception ex)
                    {
                        return Unauthorized();
                    }
                    if (user?.Role == Role.Patron)
                    {
                        var request = new ReserveBookCommand() { BookId = bookId, UserId = user.Id };
                        ValidationResult validationResult = _reserveBookCommandValidator.Validate(request);
                        if (!validationResult.IsValid)
                        {
                            return BadRequest(validationResult.Errors);
                        }
                        return await _mediator.Send(request);
                    }
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize(Policy = "LibrarianRequired")]
        public async Task<IActionResult> Reservations([FromBody] BrowseReservationsQuery browseReservationsQuery)
        {
            var (paginationMetadata, reservations) = await _mediator.Send(browseReservationsQuery);
            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(paginationMetadata));
            return Ok(reservations);
        }


        [HttpPost("approve/{reservationId}")]
        [Authorize(Policy = "LibrarianRequired")]
        public async Task<IActionResult> ApproveReservation(int reservationId)
        {
            var request = new ApproveReservationCommand() { ReservationId = reservationId };
            ValidationResult validationResult = _approveReservationValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            return await _mediator.Send(request);
        }

    }
}
