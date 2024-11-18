using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services;

public class StudentQuizService
{
    private readonly QuizContext _context;

    public StudentQuizService(QuizContext context)
    {
        _context = context;
    }

    public async Task<QuizListDto[]> GetActiveQuizzesAsync(Guid categoryId)
    {
        var query = _context.Quizzes.Where(q => q.IsActive);
        if (categoryId !=Guid.Empty)
        {
            query = query.Where(q => q.CategoryId == categoryId);
        }

        var quizzes = await query.Select(q => new QuizListDto
        {
            Id = q.Id,
            CategoryId = q.CategoryId,
            CategoryName = q.Category.Name,
            Name = q.Category.Name,
            TimeInMinutes = q.TimeInMinutes,
            TotalQuestions = q.TotalQuestions
        }).ToArrayAsync();
        
        return quizzes;
    }
}