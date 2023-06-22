using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IReadingListRepository
    {
        public Task<(PaginationMetadata, List<ReadingList>)> GetReadingListsAsync(int pageNumber, int pageSize, int userId);
        Task<ReadingList?> GetReadingListAsync(int id);
        Task<ReadingList?> GetReadingListIncludingCollectionsAsync(int id);
        Task<ReadingList> AddReadingListAsync(ReadingList readingList);
        Task<ReadingList> UpdateReadingListAsync(ReadingList readingList);
        Task RemoveReadingListAsync(ReadingList readingList);
    }
}
