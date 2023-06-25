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

namespace Application.Entities.Feedbacks.Handlers
{
    public class EditFeedbackCommandHandler : IRequestHandler<EditFeedbackCommand,FeedbackDto>
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

        public async Task<FeedbackDto> Handle(EditFeedbackCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving feedback with ID {request.Id}");
            var original = await _feedbackRepository.GetFeedbackByIdAsync(request.Id);
            if (original == null)
            {
                _logger.LogError($"Feedback NOT FOUND with ID {request.Id}");
                return null;
            }

            _logger.LogInformation($"Retrieving user with ID {request.UserId}");
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null)
            {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return null;
            }

            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            if (await _bookRepository.GetBookByIdAsync(request.BookId) == null)
            {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return null;
            }
                
            if(original.UserId != request.UserId)
                return null;

            original.Rating = request.Rating;
            original.CreatedDate = request.CreatedDate;
            original.Review = request.Review;
            original.UserId = request.UserId;
            original.BookId = request.BookId;

            _logger.LogInformation($"Updating feedback with Id {original.Id}");
            var updated = await _feedbackRepository.UpdateFeedbackAsync(original);
            return _mapper.Map<FeedbackDto>(updated);
        }
    }
}
