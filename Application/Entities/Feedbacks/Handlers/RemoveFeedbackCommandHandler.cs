using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Feedbacks.Commands;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Feedbacks.Handlers
{
    public class RemoveFeedbackCommandHandler : IRequestHandler<RemoveFeedbackCommand,bool>
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

        public async Task<bool> Handle(RemoveFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return false;
            }

            _logger.LogInformation($"Retrieving feedback with ID {request.FeedbackId}");
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(request.FeedbackId);
            if ( feedback == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return false;
            }
            if ( feedback.UserId != request.UserId)
            {
                _logger.LogError($"User is not the owner of the feedback with Id {feedback.Id}");
                return false;
            }
            _logger.LogInformation($"Removing feedback with ID {feedback.Id}");
            await _feedbackRepository.RemoveFeedbackAsync(feedback);
            return true;
        }
    }
}
