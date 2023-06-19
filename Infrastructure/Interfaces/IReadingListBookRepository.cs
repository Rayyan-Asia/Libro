namespace Infrastructure.Interfaces
{
    public interface IReadingListBookRepository
    {
        Task RemoveBooksFromReadingList(int listId);
    }
}