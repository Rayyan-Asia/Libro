using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Entities.Feedbacks.Commands;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Feedbacks.Handlers
{
    public class RemoveFeedbackCommandHandler : IRequestHandler<RemoveFeedbackCommand,bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFeebackRepository _feedbackRepository;

        public RemoveFeedbackCommandHandler(IUserRepository userRepository, IFeebackRepository feedbackRepository)
        {
            _userRepository = userRepository;
            _feedbackRepository = feedbackRepository;
        }

        public async Task<bool> Handle(RemoveFeedbackCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null)
                return false;
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(request.FeedbackId);
            if ( feedback == null || feedback.UserId != request.UserId)
                return false;
            await _feedbackRepository.RemoveFeedbackAsync(feedback);
            return true;
        }
    }
}
