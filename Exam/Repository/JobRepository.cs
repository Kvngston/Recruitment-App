using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Data;
using Exam.Domains;
using Exam.Enums;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Exam.Repository
{
    public class JobRepository: IJobService
    {
        public IApplicationService ApplicationService { get; }
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public JobRepository(ApplicationDbContext dbContext, IApplicationService applicationService, IUserService userService, IEmailSender emailSender)
        {
            ApplicationService = applicationService;
            _dbContext = dbContext;
            _userService = userService;
            _emailSender = emailSender;
        }
        
        public void CreateJob(JobViewModel viewModel)
        {
            var job = new Job()
            {
                JobDescription = viewModel.JobDescription,
                JobPosition = viewModel.JobPosition,
                JobType = viewModel.JobType.ToUpper(),
                BrandOrCompanyName = viewModel.BrandOrCompanyName,
                Applications = new List<Application>(),
            };

            _dbContext.Jobs.Add(job);
            _dbContext.SaveChanges();
        }

        public void ApplyForJob(string userId, string jobId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUser.Id == userId);
            var applicationUser = _dbContext.Users.FirstOrDefault(identityUser => identityUser.Id == userId);
            if (user == null)
            {
                throw new Exception("User not Found");
            }

            var job = _dbContext.Jobs.FirstOrDefault(job1 => job1.Id == jobId);

            if (job == null)
            {
                throw new Exception("Job Not found");
            }
            
            
            // check for previous applications to ensure user hasn't applied twice 
            var previousApplication = _dbContext.Applications
                .Where(x => x.UserId.Equals(user.Id))
                .Any(y => y.JobId == jobId);


            if (previousApplication)
            {
                throw new Exception("User has applied for this job already");
            }
            
            if (user.ResumeId == null)
            {
                throw new Exception("User has no resume");
            }
            
            //Create user Application
            var userApplication = ApplicationService.CreateApplication(user, job);
            
            job.Applications.Add(userApplication);
            _dbContext.Entry(job).State = EntityState.Modified;
            _dbContext.SaveChanges();
            
            
            //Add User Application to list of User Applications
            _userService.AddUserApplication(user,userApplication);
            
            //send mail to them about applying for the job
            if (applicationUser != null)
                _emailSender.SendEmailAsync(applicationUser.Email, "Job Application",
                    "You just applied for a job, just back at a later time. Results will be communicated").Wait();
        }

        public List<Job> GetListOfJobs()
        {
            return _dbContext.Jobs.ToList();
        }

        public Job ViewParticularJob(string jobId)
        {
            var job = _dbContext.Jobs.FirstOrDefault(job1 => job1.Id == jobId);

            if (job == null)
            {
                throw new Exception("Job not found");
            }

            return job;
        }

        public Tuple<List<User>, string> ViewJobApplicants(string jobId)
        {
            var job = _dbContext.Jobs.FirstOrDefault(job1 => job1.Id == jobId);

            if (job == null)
            {
                throw new Exception("Job not found");
            }
            
            
            //get users that applied for the job
            List<User> users = new List<User>();

            
            //select all those with the job id and get the respective user corresponding to the application
            
            var applications =  _dbContext.Applications
                .Where(x => x.JobId == jobId && x.ApplicationStatus == ApplicationStatus.PENDING);
            foreach (var application in applications)
            {
                var user = _dbContext.ApplicationUsers.FirstOrDefault(user1 => user1.Id == application.UserId);
                users.Add(user);
            }

            return new Tuple<List<User>, string>(users,jobId);
        }

        public Tuple<User, Resume, string> ViewApplicant(int id, string jobId)
        {
            var user = _dbContext.ApplicationUsers.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var resume = _dbContext.Resumes.FirstOrDefault(x => x.Id == user.ResumeId);

            if (resume == null)
            {
                throw new Exception("Resume Not Found");
            }
            
            return new Tuple<User,Resume, string>(user,resume, jobId);
        }

        public void RejectApplicant(int id, string jobId)
        {
            var user = ViewApplicant(id, jobId);
            var job = _dbContext.Jobs.FirstOrDefault(x => x.Id == jobId);
            var applicationUser = _dbContext.Users.FirstOrDefault(x => x.Id == user.Item1.ApplicationUserId);

            if (applicationUser == null)
            {
                throw new Exception("Identity User not found, Please log in");
            }

            if (job == null)
            {
                throw new Exception("Job Not Found");
            }
            
            var userApplication = ApplicationService.GetUserSpecificApplication(user.Item1.ApplicationUserId, jobId);

            userApplication.ApplicationStatus = ApplicationStatus.REJECTED;

            _dbContext.Entry(userApplication).State = EntityState.Modified;
            _dbContext.SaveChanges();

            string message = $@"Thank you very much for your interest in employment opportunities with {job.BrandOrCompanyName}

                This message is to inform you that we have selected a candidate who is a match for the job requirements of the position.

                We appreciate you taking the time to apply for employment with our company and wish you the best of luck in your future endeavors.

                Best regards,
                
                TkCodes
                Hiring Manager";

            _emailSender.SendEmailAsync(applicationUser.Email, "Job Application Status", message);

        }

        public void AcceptApplicant(int id, string jobId)
        {
            var user = ViewApplicant(id, jobId);
            var job = _dbContext.Jobs.FirstOrDefault(x => x.Id == jobId);
            var applicationUser = _dbContext.Users.FirstOrDefault(x => x.Id == user.Item1.ApplicationUserId);

            if (applicationUser == null)
            {
                throw new Exception("Identity User not found, Please log in");
            }

            if (job == null)
            {
                throw new Exception("Job Not Found");
            }
            
            var userApplication = ApplicationService.GetUserSpecificApplication(user.Item1.ApplicationUserId, jobId);

            userApplication.ApplicationStatus = ApplicationStatus.ACCEPTED;

            _dbContext.Entry(userApplication).State = EntityState.Modified;
            _dbContext.SaveChanges();

            string message = $@"Dear {user.Item1.FirstName} {user.Item1.LastName},
 
We are excited to be offering you a full-time position as a {job.JobPosition} at {job.BrandOrCompanyName}. Based on your experience, we are looking forward to seeing how you will take our brand to the next level.
 
If you decide to accept this role, your anticipated start date will be communicated soon. You will be expected to work 40 hours per week, Monday through Friday with the option to work remotely up to two days per week. Please find attached an updated copy of the job description to familiarize yourself with some of the position’s duties and responsibilities.
 
As an employee of {job.BrandOrCompanyName}, you will also have access to our comprehensive benefits program, which includes unlimited vacation days, health insurance, 401(k) with company matching, and tuition reimbursement. I have attached the full details of the benefits we offer for you to look over.
 
Please note that {job.BrandOrCompanyName} is offering you employment on an at-will basis. This means that we may end your employment at any time without cause. You are also free to leave the company at any time for any reason.
 
I will get you started with the rest of the on-boarding process.
 
We are excited about the possibility of you joining {job.BrandOrCompanyName}!
 
Sincerely,
 
TkCodes
Hiring Manager";

            _emailSender.SendEmailAsync(applicationUser.Email, "Job Application Status", message);
        }
    }
}