using Domain;

namespace Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> AddReservationAsync(Reservation reservation);
        Task<Reservation> UpdateReservationAsync(Reservation reservation);
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<Reservation?> GetReservationByUserIdAndBookIdAsync(int userId, int bookId);
        Task<(PaginationMetadata, List<Reservation>)> GetAllReservationsAsync(int pageNumber, int pageSize);
        Task<(PaginationMetadata, List<Reservation>)> GetAllReservationsByUserIdAsync(int pageNumber, int pageSize, int userId);
        Task<Reservation?> ApproveReservationByIdAsync(int reservationId);
        Task<bool> IsPatronEligableForReservationAsync(int userId);
        Task<Reservation?> RejectReservationByIdAsync(int reservationId);
    }
}