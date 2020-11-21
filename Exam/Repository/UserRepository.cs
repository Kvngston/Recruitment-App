using System;
using System.Collections.Generic;
using System.Linq;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Exam.Areas.Identity.Pages.Account;
using Exam.Data;
using Exam.Domains;
using Exam.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exam.Repository
{
    public class UserRepository : IUserService
    {
        public ICloudinaryInitialization CloudinaryInitialization { get; }
        public IEmailSender EmailSender { get; }
        private readonly ApplicationDbContext _dbContext;
        private readonly IApplicationService _applicationService;
        private readonly IResumeService _resumeService;

        public UserRepository(ApplicationDbContext dbContext, ICloudinaryInitialization cloudinaryInitialization, IEmailSender emailSender, IApplicationService applicationService, IResumeService resumeService)
        {
            CloudinaryInitialization = cloudinaryInitialization;
            EmailSender = emailSender;
            _dbContext = dbContext;
            _applicationService = applicationService;
            _resumeService = resumeService;
        }
         
        public void CreateUser(RegisterModel.InputModel viewModel, string userId)
        {
            var cloudinary = CloudinaryInitialization.Initialize();
            
            if (viewModel.ProfileImage.Name == null)
            {
                throw new Exception("Image not found, Please add an image");
            }
            
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(viewModel.ProfileImage.FileName, viewModel.ProfileImage.OpenReadStream()),
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            var applicationUser = _dbContext.Users.FirstOrDefault(identityUser => identityUser.Id == userId);

            var user = new User()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Age = viewModel.Age,
                ApplicationUserId = userId,
                ApplicationUser = applicationUser,
                Resume = null,
                ImageLocation = uploadResult.SecureUrl.AbsoluteUri
            };

            _dbContext.ApplicationUsers.Add(user);
            _dbContext.SaveChanges();
        }

        public void AddUserApplication(User user, Application application)
        {
            user.UserJobApplications.Add(application);
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public Tuple<List<Application>, User, List<WorkExperience>> ReturnUserDetails(string userId)
        {
            //get user details for the dashboard
            var userApplications = _applicationService.GetUserApplications(userId);
            var userWorkExperiences = _resumeService.GetUserWorkExperiences(userId);
            var applicationUser = _dbContext.ApplicationUsers.FirstOrDefault(identityUser => identityUser.ApplicationUser.Id == userId);
            
           return new Tuple<List<Application>, User, List<WorkExperience>>(userApplications,applicationUser,userWorkExperiences);
        }
    }
}