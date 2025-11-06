using Microsoft.EntityFrameworkCore;

namespace CoolJob.Models
{
    public class CoolJobContext : DbContext
    {
        public CoolJobContext(DbContextOptions<CoolJobContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Applications)
                .WithOne(a => a.User)          
                .HasForeignKey(a => a.UserId)
                .IsRequired();                 

            modelBuilder.Entity<Job>()
                .HasMany(j => j.Applications)
                .WithOne(a => a.Job)           
                .HasForeignKey(a => a.JobId)
                .IsRequired();                 
            
            modelBuilder.Entity<Employer>()
                .HasMany(e => e.Jobs)
                .WithOne(j => j.Employer)     
                .HasForeignKey(j => j.EmployerId)
                .IsRequired();                 
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Skills)
                .WithMany(s => s.Users);
            
            modelBuilder.Entity<Application>()
                .Property(a => a.CoverLetter)
                .IsRequired(false); 
        }
    }
}