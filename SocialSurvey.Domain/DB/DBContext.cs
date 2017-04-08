using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialSurvey.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public DbSet<Form> Forms { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User Table
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

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
            
            //Form Table
            modelBuilder.Entity<Form>()
                .HasKey(f => f.FromId);
            modelBuilder.Entity<Form>()
                .Property(f => f.FromId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Form>()
                .HasOne(f => f.Survey)
                .WithMany(s => s.Forms)
                .HasForeignKey(f => f.SurveyId);
            modelBuilder.Entity<Form>()
                .HasOne(f => f.User)
                .WithMany(u => u.Forms)
                .HasForeignKey(f => f.UserId);

            //Answer table
            modelBuilder.Entity<Answer>()
                .HasKey(a => a.AnswerId);
            modelBuilder.Entity<Answer>()
                .Property(a => a.AnswerId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Form)
                .WithMany(f => f.Answers)
                .HasForeignKey(a => a.FormId);
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);

            //AnswerOption table
            modelBuilder.Entity<AnswerOption>()
                .HasKey(t => new { t.AnswerId, t.OptionId });
            modelBuilder.Entity<AnswerOption>()
                .HasOne(ao => ao.Answer)
                .WithMany(a => a.AnswerOptions)
                .HasForeignKey(ao => ao.AnswerId);

            modelBuilder.Entity<AnswerOption>()
                .HasOne(ao => ao.Option)
                .WithMany(o => o.AnswerOptions)
                .HasForeignKey(ao => ao.OptionId);

            //need for ef, off cascade deleting, need ti rewrite
            modelBuilder.Entity<Question>()
                .HasMany(h => h.Answers)
                .WithOne(w => w.Question)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehav‌​ior.Restrict);
            modelBuilder.Entity<Option>()
                .HasMany(h => h.AnswerOptions)
                .WithOne(w => w.Option)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehav‌​ior.Restrict);
            base.OnModelCreating(modelBuilder);
        }

    }
}
