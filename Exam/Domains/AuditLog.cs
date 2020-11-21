﻿﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 using Microsoft.AspNetCore.Identity;

namespace Exam.Domains
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Action { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        
    }
}