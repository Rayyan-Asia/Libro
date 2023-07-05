using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class JobRepository : IJobRepository
    {
        private readonly LibroDbContext _context;

        public JobRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
           _context.Jobs.Add(job);
           await _context.SaveChangesAsync();
           return job;
        }

        public async Task<Job?> GetJobByIdAsync(int JobId)
        {
            return await _context.Jobs.SingleOrDefaultAsync(j => j.Id == JobId);
        }
    }
}
