using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Exam.Models
{
    public class AdvertisementViewModel
    {
        [Required]
        public string AdvertName { get; set; }

        [Required]
        [Display(Name = "Brand Name")]
        public string AdvertCompanyOrBrand { get; set; }
        
        [Required]
        public string AdvertLink { get; set; }
        
        [Required]
        public IFormFile AdvertImage { get; set; }
    }
    
}