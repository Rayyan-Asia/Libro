using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Feedbacks.Handlers
{
    public class EditFeedbackCommandHandler : IRequestHandler<EditFeedbackCommand,IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EditFeedbackCommandHandler> _logger;

        public EditFeedbackCommandHandler(IUserRepository userRepository, IBookRepository bookRepository, IFeebackRepository feedbackRepository,
            IMapper mapper, ILogger<EditFeedbackCommandHandler> logger)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(EditFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving feedback with ID {request.Id}");
            var original = await _feedbackRepository.GetFeedbackByIdAsync(request.Id);
            if (original == null)
            {
                _logger.LogError($"Feedback NOT FOUND with ID {request.Id}");
                return new NotFoundObjectResult($"Feedback NOT FOUND with ID {request.Id}"); // Return a 404 Not Found response
            }

            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return new NotFoundObjectResult($"User NOT FOUND with ID {request.UserId}"); // Return a 404 Not Found response
            }

            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return new NotFoundObjectResult($"Book NOT FOUND with ID {request.BookId}");
            }

            if (original.UserId != request.UserId)
                return new ForbidResult("User ID does not match ID of Creator");

            original.Rating = request.Rating;
            original.CreatedDate = request.CreatedDate;
            original.Review = request.Review;
            original.UserId = request.UserId;
            original.BookId = request.BookId;

            _logger.LogInformation($"Updating feedback with Id {original.Id}");
            var updated = await _feedbackRepository.UpdateFeedbackAsync(original);

            return new OkObjectResult(_mapper.Map<FeedbackDto>(updated)); // Return a 200 OK response with the updated feedback data
        }
    }
}
