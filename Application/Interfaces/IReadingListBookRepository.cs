namespace Application.Interfaces
{
    public interface IReadingListBookRepository
    {
        Task RemoveBooksFromReadingList(int listId);
    }
}