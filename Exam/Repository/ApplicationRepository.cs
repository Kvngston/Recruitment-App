using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Data;
using Exam.Domains;
using Exam.Service;
using Microsoft.EntityFrameworkCore;

namespace Exam.Repository
{
    public class ApplicationRepository: IApplicationService
    {
        
        private readonly ApplicationDbContext _dbContext;

        public ApplicationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public Application CreateApplication(User user, Job job)
        {

            var userApplication = new Application()
            {
                Applicant = user,
                Job = job,
                JobId = job.Id,
                ApplicantResume = user.Resume,
                ResumeId = user.ResumeId,
                UserId = user.Id,
            };

            _dbContext.Applications.Add(userApplication);
            _dbContext.SaveChanges();

            return userApplication;
        }

        public List<Application> GetUserApplications(string userId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _dbContext.Applications.Include("Job").Where(application => application.UserId == user.Id).ToList();
        }

        public Application GetUserSpecificApplication(string userId, string jobId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _dbContext.Applications.FirstOrDefault(x => x.UserId == user.Id && x.JobId == jobId);
        }
    }
}