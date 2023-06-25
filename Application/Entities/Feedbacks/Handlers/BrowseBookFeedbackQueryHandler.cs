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
    public class BrowseBookFeedbackQueryHandler : IRequestHandler<BrowseBookFeedbackQuery, (PaginationMetadata, List<FeedbackDto>)>
    {
        private readonly IMapper _mapper;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BrowseBookFeedbackQueryHandler> _logger;

        public BrowseBookFeedbackQueryHandler(IMapper mapper, IFeebackRepository feedbackRepository, IBookRepository bookRepository, ILogger<BrowseBookFeedbackQueryHandler> logger)
        {
            _mapper = mapper;
            _feedbackRepository = feedbackRepository;
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<(PaginationMetadata, List<FeedbackDto>)> Handle(BrowseBookFeedbackQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book with ID {request.BookId}");
            if (await _bookRepository.GetBookByIdAsync(request.BookId) == null) {
                _logger.LogError($"Book NOT FOUND with ID {request.BookId}");
                return (null, null); 
            }
            _logger.LogInformation($"Retrieving reviews for book with ID {request.BookId}");
            var (paginationMetadata, feedbacks) = await _feedbackRepository.BrowseFeedbackByBookAsync(request.pageNumber, request.pageSize, request.BookId);
            var dtoList = _mapper.Map<List<FeedbackDto>>(feedbacks);
            return (paginationMetadata, dtoList);
        }
    }
}
