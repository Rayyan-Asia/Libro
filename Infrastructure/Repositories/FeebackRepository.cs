﻿using System;
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
    public class FeebackRepository : IFeebackRepository
    {
        private readonly LibroDbContext _context;

        public FeebackRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<(PaginationMetadata, List<Feedback>)> BrowseFeedbackByBookAsync(int pageNumber, int pageSize, int bookId)
        {
            var feedbacks = await _context.Feedbacks
              .AsNoTracking()
              .Where(f => f.BookId == bookId)
              .Include(F=>F.User)
              .Include(F => F.Book)
              .ToListAsync();

            var count = feedbacks.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredFeedbacks = feedbacks.OrderBy(b => b.CreatedDate)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredFeedbacks);
        }

        public async Task<(PaginationMetadata, List<Feedback>)> BrowseFeedbackByUserAsync( int pageNumber, int pageSize, int userId)
        {
            var feedbacks = await _context.Feedbacks
              .AsNoTracking()
              .Where(f => f.UserId == userId)
              .Include(F => F.User)
              .Include(F => F.Book)
              .ToListAsync();

            var count = feedbacks.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredFeedbacks = feedbacks.OrderBy(b => b.CreatedDate)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredFeedbacks);
        }

        public async Task<Feedback?> GetFeedbackByIdAsync(int id)
        {
            return await _context.Feedbacks.SingleOrDefaultAsync(b => b.Id == id);
        }
        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task<Feedback> UpdateFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }

        public async Task RemoveFeedbackAsync(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }
}
