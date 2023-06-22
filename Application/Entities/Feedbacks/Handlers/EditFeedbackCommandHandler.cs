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
    public class EditFeedbackCommandHandler : IRequestHandler<EditFeedbackCommand,FeedbackDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IFeebackRepository _feedbackRepository;
        private readonly IMapper _mapper;

        public EditFeedbackCommandHandler(IUserRepository userRepository, IBookRepository bookRepository, IFeebackRepository feedbackRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
        }

        public async Task<FeedbackDto> Handle(EditFeedbackCommand request, CancellationToken cancellationToken)
        {
            var original = await _feedbackRepository.GetFeedbackByIdAsync(request.Id);
            if (original == null)
                return null;
            if (await _userRepository.GetUserByIdAsync(request.UserId) == null)
                return null;
            if (await _bookRepository.GetBookByIdAsync(request.BookId) == null)
                return null;
            if(original.UserId != request.UserId)
                return null;

            original.Rating = request.Rating;
            original.CreatedDate = request.CreatedDate;
            original.Review = request.Review;
            original.UserId = request.UserId;
            original.BookId = request.BookId;
            

            var updated = await _feedbackRepository.UpdateFeedbackAsync(original);
            return _mapper.Map<FeedbackDto>(updated);
        }
    }
}
