using Application.Interfaces;
using Application.Services;
using AutoDependencyRegistration.Attributes;
using Domain;
using Hangfire;
using Hangfire.Storage;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    [RegisterClassAsScoped]
    public class JobService : IJobService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IJobRepository _jobRepository;

        public JobService(IUserRepository userRepository, IMailService mailService, IJobRepository jobRepository)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _jobRepository = jobRepository;
        }

        public async Task<bool> JobExists(string jobId)
        {
            // Retrieve job information using Hangfire's JobStorage.Current property
            using (var connection = JobStorage.Current.GetConnection())
            {
                // Check if the job exists based on its ID
                JobData jobData = connection.GetJobData(jobId);

                return jobData != null;
            }
        }

        public async Task RemoveJobsAsync(List<Job> jobs)
        {
            foreach (var job in jobs)
            {
                if (await JobExists(job.BackgroundJobId))
                {
                    BackgroundJob.Delete(job.BackgroundJobId);
                }

            }
        }

        public async Task CreateJobs(int reservationId, Book book, Loan loan)
        {
            var newGenres = new List<Genre>();
            var newAuthors = new List<Author>();

            // Assign values from existing lists to new instances
            foreach (var genre in book.Genres)
            {
                newGenres.Add(new Genre { Id = genre.Id, Type = genre.Type });
            }

            foreach (var author in book.Authors)
            {
                newAuthors.Add(new Author { Id = author.Id, Name = author.Name });
            }

            // Use the new lists with detached entities for further processing
            book.Genres = newGenres;
            book.Authors = newAuthors;
            var week = DateTime.Now.AddDays(21);
            var day = DateTime.Now.AddDays(27);
            var due = DateTime.Now.AddDays(28);
            var overdue = DateTime.Now.AddDays(29);
            var stringId = BackgroundJob.Schedule(() => SendLoanWeekBeforeDueEmail(loan.UserId, book), week);
            Job job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanDayBeforeDueEmail(loan.UserId, book), day);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanDayDueEmail(loan.UserId, book), due);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

            stringId = BackgroundJob.Schedule(() => SendLoanOverdueEmail(loan.UserId, book), overdue);
            job = await _jobRepository.AddJobAsync(new Job { BackgroundJobId = stringId });
            loan.Jobs.Add(job);

        }


        public async Task SendLoanWeekBeforeDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan Due Date From Libro";
            string body = $"Hello There {user.Name},\nYou have one week left with the book {book.Title}.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }
        public async Task SendLoanDayBeforeDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan Due Date From Libro";
            string body = $"Hello There {user.Name},\nYou have one day left with the book {book.Title}.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }

        public async Task SendLoanDayDueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Last day to return book to Libro";
            string body = $"Hello There {user.Name},\nThe book: {book.Title} is due for a return today.\nPlease come to your nearest Libro station for returning it before your time runs out";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }

        public async Task SendLoanOverdueEmail(int userId, Book book)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);
            string to = user.Name;
            string toAddress = user.Email;
            string subject = "Book Loan over due Libro";
            string body = $"Hello There {user.Name},\nThe book: {book.Title} is overdue for a return.\nPlease come to your nearest Libro station for returning it, and be cautious because there will be charges for overdue books";
            await _mailService.SendEmailAsync(toAddress, to, subject, body);
        }


    }
}
