using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Hangfire;
using Hangfire.Storage;

namespace Infrastructure
{
    [RegisterClassAsScoped]
    public class JobService : IJobService
    {
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
    }
}
