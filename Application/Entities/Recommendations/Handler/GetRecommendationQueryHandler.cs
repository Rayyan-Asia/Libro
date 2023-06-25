using Application.DTOs;
using Application.Entities.Recommendations.Query;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Recommendations.Handler
{
    public class GetRecommendationQueryHandler : IRequestHandler<GetRecommendationQuery, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRecommendationQueryHandler> _logger;

        public GetRecommendationQueryHandler(IBookRepository bookRepository, IMapper mapper, ILogger<GetRecommendationQueryHandler> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BookDto> Handle(GetRecommendationQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Retrieving book recommendation for user with ID {request.UserId}");
            var result = await _bookRepository.GetRecommendedBookAsync(request.UserId);
            return _mapper.Map<BookDto>(result);
        }
    }
}
