using System.Collections.Generic;
using System.Threading.Tasks;
using Exam.Domains;
using Exam.Models;
using Microsoft.AspNetCore.Http;

namespace Exam.Service
{
    public interface IResumeService
    {
        void CreateResume(ResumeViewModel viewModel, string userId);
        Task<byte[]> UploadResume(IFormFile formFile);

        Resume GetUserResume(string userId);

        EditResumeViewModel MapResumeToEditResumeViewModel(string userId);

        void EditResume(EditResumeViewModel viewModel, string userId);

        List<WorkExperience> GetUserWorkExperiences(string userId);

        void AddWorkExperience(WorkExperienceViewModel workExperienceViewModel, string userId);

        void GetUploadedResume(string userId);
    }
}