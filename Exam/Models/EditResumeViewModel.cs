using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Exam.Models
{
    public class EditResumeViewModel
    {
        [Required]
        public string BriefSelfDescription { get; set; }
        [Required]
        public string Skills { get; set; }

        [Required]
        public IFormFile ResumePdf { get; set; }
    }
}