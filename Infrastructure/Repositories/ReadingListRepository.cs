using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadingListRepository : IReadingListRepository
    {
        private readonly LibroDbContext _context;

        public ReadingListRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<ReadingList> AddReadingListAsync(ReadingList readingList)
        {
            _context.ReadingLists.Add(readingList);
            await _context.SaveChangesAsync();
            return readingList;
        }

        public async Task RemoveReadingListAsync(ReadingList readingList)
        {
            _context.ReadingLists.Remove(readingList);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadingList?> GetReadingListAsync(int id)
        {
            return await _context.ReadingLists.SingleOrDefaultAsync(r=>r.Id == id);
        }

        public async Task<(PaginationMetadata, List<ReadingList>)> GetReadingListsAsync(int pageNumber, int pageSize, int userId)
        {
            var readingLists = await _context.ReadingLists
              .AsNoTracking()
              .Where(r => r.UserId == userId)
              .Include(a => a.Books)
              .Select(a => new ReadingList
              {
                  Id = a.Id,
                  Name = a.Name,
                  Description = a.Description,
                  Books = a.Books.Select(b => new Book
                  {
                      Id = b.Id,
                      Title = b.Title,
                  }).ToList(),
              })
              .ToListAsync();

            var count = readingLists.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredReadingList = readingLists.Skip(pageNumber * pageSize)
                .Take(pageSize).ToList();
            return (metadata, filteredReadingList);
        }

        public async Task<ReadingList> UpdateReadingListAsync(ReadingList readingList)
        {
            _context.ReadingLists.Update(readingList);
            await _context.SaveChangesAsync();
            return readingList;
        }

        public async  Task<ReadingList?> GetReadingListIncludingCollectionsAsync(int id)
        {
            return await _context.ReadingLists.Include(b=>b.Books).SingleOrDefaultAsync(r => r.Id == id);
        }
    }
}
