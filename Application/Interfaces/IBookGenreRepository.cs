namespace Application.Interfaces
{
    public interface IBookGenreRepository
    {
        Task RemoveGenresFromBook(int bookId);
    }
}
