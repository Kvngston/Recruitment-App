using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data;
using Exam.Domains;
using Exam.Models;
using Exam.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.Repository
{
    public class ResumeRepository : IResumeService
    {
        public ApplicationDbContext DbContext { get; }

        public ResumeRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        //Creates Users Resume 
        //Adds Work Experience
        //Adds Education background
        //Adds Achievement
        //Adds user skills 
        //Saves Changes to db
        public void CreateResume(ResumeViewModel viewModel, string userId)
        {
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var workExperience = new WorkExperience()
            {
                CompanyName = viewModel.CompanyName,
                PositionName = viewModel.PositionName,
                TimeDuration = viewModel.TimeDuration,
                SummaryOfWorkDone = viewModel.SummaryOfWorkDone,
                EmploymentType = viewModel.EmploymentType,
                User = user,
                UserId = user.Id
            };

            DbContext.WorkExperiences.Add(workExperience);
            DbContext.SaveChanges();

            var achievement = new Achievement()
            {
                AchievementName = viewModel.AchievementName,
                AchievementYear = viewModel.AchievementYear,
                User = user,
                UserId = user.Id
            };

            DbContext.Achievements.Add(achievement);
            DbContext.SaveChanges();

            var education = new Education()
            {
                CertificateAttained = viewModel.CertificateAttained,
                SchoolLocation = viewModel.SchoolLocation,
                SchoolName = viewModel.SchoolName,
                TimeFrame = viewModel.TimeFrame,
                User = user,
                UserId = user.Id
            };

            DbContext.Educations.Add(education);
            DbContext.SaveChanges();


            //physical Resume file upload should be done here 
            var content = UploadResume(viewModel.ResumePdf).Result;

            var resume = new Resume()
            {
                Achievements = new List<Achievement> {achievement},
                Skills = viewModel.Skills,
                BriefProfile = viewModel.BriefSelfDescription,
                EducationBackground = new List<Education> {education},
                WorkExperiences = new List<WorkExperience> {workExperience},
                PdfContent = content,
                FileExtensionName = Path.GetExtension(viewModel.ResumePdf.FileName)
            };

            DbContext.Resumes.Add(resume);
            DbContext.SaveChanges();

            user.Resume = resume;
            user.ResumeId = resume.Id;
            DbContext.Entry(user).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        public async Task<byte[]> UploadResume(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                //if file is less than 2mb process, throw error if not 
                if (memoryStream.Length < 2097152)
                {
                    return memoryStream.ToArray();
                }

                throw new Exception("File Must not be Larger than 2mb");
            }
        }

        public Resume GetUserResume(string userId)
        {
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var userResume = DbContext.Resumes.FirstOrDefault(x => x.Id == user.ResumeId);

            if (userResume == null)
            {
                throw new Exception("User Resume Not Found");
            }
            
            return userResume;
        }

        public EditResumeViewModel MapResumeToEditResumeViewModel(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                
                //make this less redundant after tests
                var userResume = GetUserResume(userId);
                if (userResume == null)
                {
                    throw new Exception("User Resume not found, Redirecting user to create new Resume");
                }
                
                var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                

                //Turing stored bytes back to Resume
                var stream = new MemoryStream(userResume.PdfContent);
                var formFile = new FormFile(stream, 0, userResume.PdfContent.Length,
                    user.FirstName + user.LastName, user.FirstName + user.LastName);

               return new EditResumeViewModel()
                {
                    Skills = userResume.Skills,
                    BriefSelfDescription = userResume.BriefProfile,
                    ResumePdf = formFile
                };
            }
            throw new Exception("User id not found, Please log in");
        }

        public void EditResume(EditResumeViewModel viewModel, string userId)
        {
            var userResume = GetUserResume(userId);
            if (userResume == null)
            {
                throw new Exception("User Resume not found, Redirecting user to create new Resume");
            }
            
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            
            
            var content = UploadResume(viewModel.ResumePdf).Result;

            userResume.Skills = viewModel.Skills;
            userResume.BriefProfile = viewModel.BriefSelfDescription;
            userResume.PdfContent = content;
            userResume.FileExtensionName = Path.GetExtension(viewModel.ResumePdf.FileName);
            userResume.UpdatedAt = DateTime.Now;

            DbContext.Entry(userResume).State = EntityState.Modified;
            DbContext.SaveChanges();

            user.Resume = userResume;
            DbContext.Entry(user).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        public List<WorkExperience> GetUserWorkExperiences(string userId)
        {
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return DbContext.WorkExperiences.Where(experience => experience.UserId == user.Id).ToList();
        }

        public void AddWorkExperience(WorkExperienceViewModel viewModel ,string userId)
        {
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            
            var workExperience = new WorkExperience()
            {
                User = user,
                UserId = user.Id,
                CompanyName = viewModel.CompanyName,
                EmploymentType = viewModel.EmploymentType,
                PositionName = viewModel.PositionName,
                TimeDuration = viewModel.TimeDuration,
                SummaryOfWorkDone = viewModel.SummaryOfWorkDone
            };

            DbContext.WorkExperiences.Add(workExperience);
            DbContext.SaveChanges();
        }

        public void GetUploadedResume(string userId)
        {
            var user = DbContext.ApplicationUsers.FirstOrDefault(user1 => user1.ApplicationUserId == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var resume = DbContext.Resumes.FirstOrDefault(x => x.Id == user.ResumeId);

            if (resume == null)
            {
                throw new Exception("User has no resume");
            }

            File.WriteAllBytes($"{user.FirstName}{user.LastName}{resume.FileExtensionName}",resume.PdfContent);
        }
    }
}