namespace Application.Interfaces
{
    public interface IBookGenreRepository
    {
        Task RemoveGenresFromBookAsync(int bookId);
    }
}
