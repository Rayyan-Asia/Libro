using Application.DTOs;
using Application.Entities.Recommendations.Query;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Entities.Recommendations.Handler
{
    public class GetRecommendationQueryHandler : IRequestHandler<GetRecommendationQuery, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public GetRecommendationQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<BookDto> Handle(GetRecommendationQuery request, CancellationToken cancellationToken)
        {
            var result = await _bookRepository.GetRecommendedBookAsync(request.UserId);
            return _mapper.Map<BookDto>(result);
        }
    }
}
