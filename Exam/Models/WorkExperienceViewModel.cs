using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class WorkExperienceViewModel
    {
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
    }
}