using CareerSim.Models;
using CareerSim.Models.Career;
using CareerSim.Models.Logs;
using CareerSim.Models.Question;
using CareerSim.Models.Shape;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareerSim.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<Career> Careers { get; set; }
        public DbSet<CareerTask> CareerTasks { get; set; }
        public DbSet<CareerAnswer> CareerAnswers { get; set; }
        public DbSet<UserCareerProgress> UserCareerProgresses { get; set; }
        public DbSet<CareerFeedback> CareerFeedbacks { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<ClassicQuestion> ClassicQuestions { get; set; }
        public DbSet<MultipleChoiceQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<Shape> Shapes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.MultipleChoiceDetail)
                .WithOne(m => m.Question)
                .HasForeignKey<MultipleChoiceQuestion>(m => m.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.ClassicDetail)
                .WithOne(c => c.Question)
                .HasForeignKey<ClassicQuestion>(c => c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
