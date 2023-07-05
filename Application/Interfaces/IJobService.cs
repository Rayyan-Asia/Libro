using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IJobService
    {
        Task<bool> JobExists(string jobId);
        Task RemoveJobsAsync(List<Job> jobsIds);
    }
}
