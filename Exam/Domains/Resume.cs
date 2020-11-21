﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

 namespace Exam.Domains
{
    public class Resume
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string BriefProfile { get; set; }
        
        public string Skills { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public byte[] PdfContent { get; set; }

        public string FileExtensionName{ get; set; }
        public ICollection<WorkExperience> WorkExperiences { get; set; }

        public ICollection<Education> EducationBackground { get; set; }
        
        public ICollection<Achievement> Achievements { get; set; }
        
        // public int UserId { get; set; }
        // public User User { get; set; }

        public Resume()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}