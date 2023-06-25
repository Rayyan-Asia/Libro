using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Queries;
using AutoMapper;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Feedbacks.Handlers
{
    public class BrowseUserFeedbackQueryHandler : IRequestHandler<BrowseUserFeedbackQuery,(PaginationMetadata,List<FeedbackDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<BrowseUserFeedbackQueryHandler> _logger;

        public BrowseUserFeedbackQueryHandler(IMapper mapper, IFeebackRepository feedbackRepository, IUserRepository userRepository, ILogger<BrowseUserFeedbackQueryHandler> logger)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<FeedbackDto>)> Handle(BrowseUserFeedbackQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving User with ID {request.UserId}");
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null) {
                _logger.LogError($"User NOT FOUND with ID {request.UserId}");
                return (null, null); 
            }

            _logger.LogInformation($"Retrieving reviews of user with ID {request.UserId}");
            var (paginationMetadata, feedbacks) = await _feedbackRepository.BrowseFeedbackByUserAsync(request.pageNumber, request.pageSize, request.UserId);
            var dtoList = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return (paginationMetadata, dtoList);
        }
    }
}
