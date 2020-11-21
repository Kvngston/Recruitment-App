using System;
using System.Collections.Generic;
using System.Text;
using Exam.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
    }
}
