﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 using Microsoft.AspNetCore.Identity;

 namespace Exam.Domains
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        [Column(name: "ImagePath")]
        public string ImageLocation { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        
        public string ApplicationUserId { get; set; }
        public IdentityUser ApplicationUser { get; set; }

        [ForeignKey(nameof(Resume))]
        public string ResumeId { get; set; }
        public Resume Resume { get; set; }

        public ICollection<Application> UserJobApplications { get; set; }


        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}