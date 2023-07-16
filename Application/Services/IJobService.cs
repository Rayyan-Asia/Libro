using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Services
{
    public interface IJobService
    {
        Task<bool> JobExists(string jobId);
        Task RemoveJobsAsync(List<Job> jobsIds);

        Task CreateJobs(int reservationId, Book book, Loan loan);
        Task SendLoanWeekBeforeDueEmail(int userId, Book book);

        Task SendLoanDayBeforeDueEmail(int userId, Book book);
        Task SendLoanDayDueEmail(int userId, Book book);
        Task SendLoanOverdueEmail(int userId, Book book);
    }
}
