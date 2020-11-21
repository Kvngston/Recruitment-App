﻿﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
  using Exam.Enums;
  using Newtonsoft.Json;

  namespace Exam.Domains
{
    public class Application
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User Applicant { get; set; }

        public string ResumeId { get; set; }
        [JsonIgnore]
        public Resume ApplicantResume { get; set; }

        public string JobId { get; set; }
        public Job Job { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.PENDING;

        public Application()
        {
            UpdatedAt = DateTime.Now;
            CreatedAt = DateTime.Now;
        }
    }
}