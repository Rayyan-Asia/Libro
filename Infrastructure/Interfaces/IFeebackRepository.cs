using Domain;

namespace Infrastructure.Interfaces
{
    public interface IFeebackRepository
    {
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
        Task<(PaginationMetadata, List<Feedback>)> BrowseFeedbackByBookAsync(int pageSize, int pageNumber, int bookId);
        Task<(PaginationMetadata, List<Feedback>)> BrowseFeedbackByUserAsync(int pageSize, int pageNumber, int userId);
        Task<Feedback?> GetFeedbackByIdAsync(int id);
        Task RemoveFeedbackAsync(Feedback feedback);
        Task<Feedback> UpdateFeedbackAsync(Feedback feedback);
    }
}