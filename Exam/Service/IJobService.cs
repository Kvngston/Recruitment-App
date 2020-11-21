using System;
using System.Collections.Generic;
using Exam.Domains;
using Exam.Models;

namespace Exam.Service
{
    public interface IJobService
    {
        void CreateJob(JobViewModel viewModel);
        void ApplyForJob(string userId, string jobId);

        List<Job> GetListOfJobs();

        Job ViewParticularJob(string jobId);

        Tuple<List<User>, string> ViewJobApplicants(string jobId);

        Tuple<User, Resume, string> ViewApplicant(int id, string jobId);

        void RejectApplicant(int id, string jobId);

        void AcceptApplicant(int id, string jobId);
    }
}