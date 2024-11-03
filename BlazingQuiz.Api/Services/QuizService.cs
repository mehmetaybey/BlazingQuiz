using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services
{
    public class QuizService
    {
        private readonly QuizContext _context;

        public QuizService(QuizContext context)
        {
            _context = context;
        }
        public async Task<QuizApiResponse> SaveQuizAsync(QuizSaveDto dto)
        {
            var questions = dto.Question.Select(q => new Question
            {
                Id = Guid.NewGuid(),
                Text = q.Text,
                Options = q.Option.Select(o => new Options
                {
                    Id = 0,
                    Text = o.Text,
                    IsCorrect = o.IsCorrect
                }).ToArray()
            }).ToArray();

            if (dto.Id == Guid.Empty)
            {

                var quiz = new Quiz
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    CategoryId = dto.CategoryId,
                    TotalQuestions = dto.TotalQuestions,
                    TimeInMinutes = dto.TimeInMinutes,
                    IsActive = dto.IsActive,
                    Questions = questions
                };
                _context.Quizzes.Add(quiz);
            }
            else
            {
                var dbQuiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == dto.Id);
                if (dbQuiz == null)
                {
                    return QuizApiResponse.Fail("Quiz doesn't exists");
                }
                dbQuiz.CategoryId = dto.CategoryId;
                dbQuiz.IsActive = dto.IsActive;
                dbQuiz.Name = dto.Name;
                dbQuiz.TimeInMinutes = dto.TimeInMinutes;
                dbQuiz.TotalQuestions = dto.TotalQuestions;
                dbQuiz.Questions = questions;

                _context.Quizzes.Update(dbQuiz);
            }

            try
            {
                await _context.SaveChangesAsync();
                return QuizApiResponse.Success();
            }
            catch (Exception ex)
            {
                return QuizApiResponse.Fail(ex.Message);
            }
        }
    }
}
