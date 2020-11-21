﻿﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
  using Newtonsoft.Json;

  namespace Exam.Domains
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string JobPosition { get; set; }

        public string BrandOrCompanyName { get; set; }

        public string JobDescription { get; set; }

        public string JobType { get; set; }
        
        public DateTime  DatePosted { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Application> Applications { get; set; }

        public Job()
        {
            UpdatedAt = DateTime.Now;
            CreatedAt = DateTime.Now;
            DatePosted = DateTime.Now;
        }
    }
    
}