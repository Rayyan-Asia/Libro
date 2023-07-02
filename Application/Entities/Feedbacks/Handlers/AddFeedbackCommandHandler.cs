using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace Application.Entities.Feedbacks.Handlers
{
    public class AddFeedbackCommandHandler : IRequestHandler<AddFeedbackCommand, IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddFeedbackCommandHandler> _logger;

        public AddFeedbackCommandHandler(IUserRepository userRepository, IBookRepository bookRepository, IFeebackRepository feedbackRepository,
            IMapper mapper, ILogger<AddFeedbackCommandHandler> logger)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
        {
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
                return new NotFoundObjectResult($"Book NOT FOUND with ID {request.BookId}"); // Return a 404 Not Found response
            }

            var feedback = new Feedback()
            {
                Rating = request.Rating,
                CreatedDate = DateTime.UtcNow,
                Review = request.Review,
                UserId = user.Id,
                BookId = book.Id,
                User = user,
                Book = book,
            };
            _logger.LogInformation($"Creating feedback");
            feedback = await _feedbackRepository.AddFeedbackAsync(feedback);

            // Create a simplified version of the book for the response
            var simplifiedBook = new BookDto()
            {
                Id = feedback.Book.Id,
                Title = feedback.Book.Title,
            };

            var feedbackDto = new FeedbackDto()
            {
                Id = feedback.Id,
                Rating = feedback.Rating,
                CreatedDate = feedback.CreatedDate,
                Review = feedback.Review,
                User = _mapper.Map<UserDto>(user),
                Book = simplifiedBook,
            };

            return new OkObjectResult(feedbackDto); // Return a 200 OK response with the created feedback data
        }
    }
}
