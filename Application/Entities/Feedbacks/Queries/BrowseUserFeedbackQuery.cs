using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Infrastructure;
using MediatR;

namespace Application.Entities.Feedbacks.Queries
{
    public class BrowseUserFeedbackQuery : IRequest<(PaginationMetadata, List<FeedbackDto>)>
    {
        [Range(0, 5)]
        public int pageSize { get; set; } = 5;
        public int pageNumber { get; set; } = 0;
        public int UserId { get; set; }
    }
}
