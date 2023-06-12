using System.Text.Json;
using Application;
using Application.Entities.Reservations.Commnads;
using Application.Entities.Reservations.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : Controller
    {

        private readonly IMediator _mediator;

        public ReservationsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
                    var user = JwtService.GetUserFromPayload(token);
                    if (user?.Role == Role.Patron)
                    {
                        var request = new ReserveBookCommand() { BookId = bookId, UserId = user.Id };
                        var result = await _mediator.Send(request);
                        if (result == null) return BadRequest();
                        return Ok(result);
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
            var result = await _mediator.Send(request);
            if (result == null) return BadRequest();
            return Ok(result);
        }

    }
}
