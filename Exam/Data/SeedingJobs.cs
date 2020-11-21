using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Domains;
using Microsoft.Extensions.DependencyInjection;

namespace Exam.Data
{
    public class SeedingJobs
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            if (!context.Jobs.Any())
            {
                var jobs = new List<Job>()
                {
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Front end Software Engineer",
                        BrandOrCompanyName = "Google",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Backend Software Engineer",
                        BrandOrCompanyName = "Google",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Front end Software Engineer",
                        BrandOrCompanyName = "Facebook",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Digital Marketer",
                        BrandOrCompanyName = "Google",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "SEO Specialist",
                        BrandOrCompanyName = "Google",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Carpenter",
                        BrandOrCompanyName = "Midas Wood Company",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Senior DevOps Engineer",
                        BrandOrCompanyName = "Deimos Cloud",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "DevSecOps Engineer",
                        BrandOrCompanyName = "Deimos Cloud",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    },
                    new Job()
                    {
                        DatePosted = DateTime.Now,
                        JobPosition = "Sys Admin",
                        BrandOrCompanyName = "Deimos Cloud",
                        JobType = "FULL TIME",
                        JobDescription = "Things Here",
                        Applications = new List<Application>(),
                    }
                };

                context.Jobs.AddRange(jobs);
                context.SaveChanges();
            }
        }
    }
}