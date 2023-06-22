using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Entities.Feedbacks.Commands;
using AutoMapper;
using Domain;
using Application.Interfaces;
using MediatR;

namespace Application.Entities.Feedbacks.Handlers
{
    public class AddFeedbackCommandHandler : IRequestHandler<AddFeedbackCommand, FeedbackDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public AddFeedbackCommandHandler(IUserRepository userRepository, IBookRepository bookRepository, IFeebackRepository feedbackRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        public async Task<FeedbackDto> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if ( user == null)
                return null;
            var book = await _bookRepository.GetBookByIdAsync(request.BookId);
            if (book == null)
                return null;
            var feedback = new Feedback()
            {
                Rating = request.Rating,
                CreatedDate = DateTime.UtcNow,
                Review = request.Review,
                UserId = user.Id,
                BookId = book.Id,
                User = user,
                Book = book,
            };

            feedback = await _feedbackRepository.AddFeedbackAsync(feedback);
            feedback.Book = new Book()
            {
                Id = feedback.Book.Id,
                Title = feedback.Book.Title,
            };
            return _mapper.Map<FeedbackDto> (feedback);
        }
    }
}
