using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Feedbacks.Handlers
{
    public class RemoveFeedbackCommandHandler : IRequestHandler<RemoveFeedbackCommand,IActionResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly ILogger<RemoveFeedbackCommandHandler> _logger;

        public RemoveFeedbackCommandHandler(IUserRepository userRepository, IFeebackRepository feedbackRepository, ILogger<RemoveFeedbackCommandHandler> logger)
        {
            _userRepository = userRepository;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(RemoveFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return new  NotFoundObjectResult($"User NOT FOUND with ID {request.UserId}"); // Return a 404 Not Found response
            }

            _logger.LogInformation($"Retrieving feedback with ID {request.FeedbackId}");
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(request.FeedbackId);
            if (feedback == null)
            {
                _logger.LogError($"Feedback NOT FOUND with ID {request.FeedbackId}");
                return new NotFoundObjectResult($"Feedback NOT FOUND with ID {request.FeedbackId}"); // Return a 404 Not Found response
            }

            if (feedback.UserId != request.UserId)
            {
                _logger.LogError($"User is not the owner of the feedback with Id {feedback.Id}");
                return new ForbidResult("User ID does not match ID of Creator");
            }

            _logger.LogInformation($"Removing feedback with ID {feedback.Id}");
            await _feedbackRepository.RemoveFeedbackAsync(feedback);
            return new NoContentResult();
        }
    }
}
