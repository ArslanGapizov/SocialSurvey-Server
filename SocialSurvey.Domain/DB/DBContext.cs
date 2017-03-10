using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialSurvey.Domain.DB
{
    public class SocialSurveyContext : DbContext
    {
        public SocialSurveyContext(DbContextOptions<SocialSurveyContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User Table
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();

            //Survey Table
            modelBuilder.Entity<Survey>()
                .HasKey(s => s.SurveyId);
            modelBuilder.Entity<Survey>()
                .Property(s => s.SurveyId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Survey>()
                .HasOne(s => s.User)
                .WithMany(u => u.Surveys)
                .HasForeignKey(s => s.UserId);

            //Question Table
            modelBuilder.Entity<Question>()
                .HasKey(q => q.QuestionId);
            modelBuilder.Entity<Question>()
                .Property(q => q.QuestionId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Survey)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SurveyId);

            //Option Table
            modelBuilder.Entity<Option>()
                .HasKey(o => o.OptionId);
            modelBuilder.Entity<Option>()
                .Property(o => o.OptionId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
