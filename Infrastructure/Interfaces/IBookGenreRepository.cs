namespace Infrastructure.Interfaces
{
    public interface IBookGenreRepository
    {
        Task RemoveGenresFromBook(int bookId);
    }
}
