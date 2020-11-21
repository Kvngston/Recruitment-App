using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class JobViewModel
    {
        [Required] 
        public string JobPosition { get; set; }

        [Required]
        public string BrandOrCompanyName { get; set; }

        [Required]
        public string JobType { get; set; }
        
        [Required]
        public string JobDescription { get; set; }
    }
}