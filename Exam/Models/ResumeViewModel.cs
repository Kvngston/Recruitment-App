using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Exam.Models
{
    public class ResumeViewModel
    {
        [Required]
        public string BriefSelfDescription { get; set; }
        [Required]
        public string Skills { get; set; }
        [Required]
        public string EmploymentType { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string PositionName { get; set; }
        [Required]
        public string TimeDuration { get; set; }
        [Required]
        public string SummaryOfWorkDone { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string SchoolLocation { get; set; }
        [Required]
        public string TimeFrame { get; set; }
        [Required]
        public string CertificateAttained { get; set; }
        [Required]
        public string AchievementName { get; set; }
        [Required]
        public string AchievementYear { get; set; }
        [Required]
        public IFormFile ResumePdf { get; set; }
    }
}