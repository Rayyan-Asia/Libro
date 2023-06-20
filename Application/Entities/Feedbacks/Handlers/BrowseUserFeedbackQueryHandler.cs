using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Queries;
using AutoMapper;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Entities.Feedbacks.Handlers
{
    public class BrowseUserFeedbackQueryHandler : IRequestHandler<BrowseUserFeedbackQuery,(PaginationMetadata,List<FeedbackDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;

        public BrowseUserFeedbackQueryHandler(IMapper mapper, IFeebackRepository feedbackRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
        }

        public async Task<(PaginationMetadata, List<FeedbackDto>)> Handle(BrowseUserFeedbackQuery request, CancellationToken cancellationToken)
        {
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null) return (null, null);
            var (paginationMetadata, feedbacks) = await _feedbackRepository.BrowseFeedbackByUserAsync(request.pageNumber, request.pageSize, request.UserId);
            var dtoList = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return (paginationMetadata, dtoList);
        }
    }
}
