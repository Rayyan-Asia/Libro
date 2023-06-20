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
    public class BrowseBookFeedbackQueryHandler : IRequestHandler<BrowseBookFeedbackQuery, (PaginationMetadata, List<FeedbackDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IBookRepository _bookRepository;

        public BrowseBookFeedbackQueryHandler(IMapper mapper, IFeebackRepository feedbackRepository, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _bookRepository = bookRepository;
        }

        public async Task<(PaginationMetadata, List<FeedbackDto>)> Handle(BrowseBookFeedbackQuery request, CancellationToken cancellationToken)
        {
            if (await _bookRepository.GetBookByIdAsync(request.BookId) == null) return (null, null);
            var (paginationMetadata, feedbacks) = await _feedbackRepository.BrowseFeedbackByBookAsync(request.pageNumber, request.pageSize, request.BookId);
            var dtoList = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return (paginationMetadata, dtoList);
        }
    }
}
