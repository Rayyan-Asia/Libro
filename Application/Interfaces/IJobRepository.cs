﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface IJobRepository
    {
        Task<Job?> GetJobByIdAsync(int JobId);
        Task<Job> AddJobAsync(Job job);
    }
}
