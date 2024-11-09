using BlazingQuiz.Api.Data.Entities;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
