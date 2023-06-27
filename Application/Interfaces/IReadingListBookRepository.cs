namespace Application.Interfaces
{
    public interface IReadingListBookRepository
    {
        Task RemoveBooksFromReadingListAsync(int listId);
    }
}