﻿using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Data.Repositories
{
    public class QuizContext : DbContext
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public QuizContext(DbContextOptions<QuizContext> options,IPasswordHasher<User> passwordHasher): base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Options> Options { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<StudentQuiz> StudentQuizzes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentQuizQuestion> StudentQuizQuestion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentQuizQuestion>()
                .HasKey(s => new { s.StudentQuizId, s.QuestionId });

            modelBuilder.Entity<StudentQuizQuestion>()
                .HasOne(s => s.StudentQuiz)
                .WithMany(s=>s.StudentQuizQuestions)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<StudentQuizQuestion>()
                .HasOne(s => s.Question)
                .WithMany(s=>s.StudentQuizQuestions)
                .OnDelete(DeleteBehavior.NoAction);

            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Name = "Mehmet Aybey",
                Email = "admin@gmail.com",
                Phone = "1234567890",
                Role = nameof(UserRole.Admin),
                IsApproved = true
            };
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, "admin");

            modelBuilder.Entity<User>()
                .HasData(adminUser);
        }
    }
}
