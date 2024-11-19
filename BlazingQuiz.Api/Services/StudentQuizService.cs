using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Api.Enums;
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

    public async Task<QuizApiResponse<Guid>> StartQuizAsync(Guid studentId,Guid quizId)
    {
        try
        {
            var studentQuiz = new StudentQuiz
            {
                StudentId = studentId,
                QuizId = quizId,
                Status = nameof(StudentQuizStatus.Started),
                StartedOn = DateTime.UtcNow
            };
            _context.StudentQuizzes.Add(studentQuiz);
            await _context.SaveChangesAsync();

            return QuizApiResponse<Guid>.Success(studentQuiz.Id);
        }
        catch (Exception e)
        {
            return QuizApiResponse<Guid>.Fail(e.Message);
        }

    }

    public async Task<QuizApiResponse<QuestionDto?>> GetNextQuestionForQuizAsync(Guid studentQuizId,Guid studentId)
    {
        var studentQuiz = await _context.StudentQuizzes
            .Include(s => s.StudentQuizQuestions)
            .FirstOrDefaultAsync(s => s.Id == studentQuizId);

        if (studentQuiz == null)
        {
            return QuizApiResponse<QuestionDto?>.Fail("Quiz does not exist");
        }

        if(studentQuiz.StudentId!= studentId)
        {
            return QuizApiResponse<QuestionDto?>.Fail("Unauthorized access");
        }

        var questionServed = await _context.StudentQuizQuestion
                                                .Select(s => s.QuestionId)
                                                .ToArrayAsync();

        var nextQuestion = await _context.Questions
                .Where(q => q.QuizId == studentQuiz.QuizId)
                .Where(q => !questionServed.Contains(q.Id))
                .OrderBy(q => Guid.NewGuid())
                .Take(1)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = q.Options.Select(o => new OptionDto
                    {
                        Id = o.Id,
                        Text = o.Text
                    }).ToList()
                })
                .FirstOrDefaultAsync();

        if (nextQuestion == null)
            return QuizApiResponse<QuestionDto?>.Fail("No more questions for this quiz");

        try
        {
            var studentQuizQuestion = new StudentQuizQuestion
            {
                StudentQuizId = studentQuizId,
                QuestionId = nextQuestion.Id
            };
            _context.StudentQuizQuestion.Add(studentQuizQuestion);
            await _context.SaveChangesAsync();
            return QuizApiResponse<QuestionDto?>.Success(nextQuestion);

        }
        catch (Exception e)
        {
            return QuizApiResponse<QuestionDto?>.Fail(e.Message);
        }

    }

    public async Task<QuizApiResponse> SaveQuestionResponseAsync(StudentQuizQuestionResponseDto dto, Guid studentId)
    {
        var studentQuiz= await _context.StudentQuizzes
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == dto.StudentQuizId);

        if (studentQuiz == null)
        {
            return QuizApiResponse.Fail("Quiz does not exist");
        }

        if (studentQuiz.StudentId != studentId)
        {
            return QuizApiResponse.Fail("Unauthorized access");
        }

        var isSelectedOptionCorrect = await _context.Options.Where(o=>o.QuestionId==dto.QuestionId && o.Id==dto.OptionId)
                                .Select(o=>o.IsCorrect)
                                .FirstOrDefaultAsync();
        if (isSelectedOptionCorrect)
        {
            studentQuiz.Score++;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return QuizApiResponse.Fail(e.Message);
            }

        }
        return QuizApiResponse.Success();
    }

    public async Task<QuizApiResponse> SubmitQuizAsync(Guid studentQuizId, Guid studentId) 
        => await CompleteQuizAsync(studentQuizId, DateTime.UtcNow, nameof(StudentQuizStatus.Completed),studentId);

    public async Task<QuizApiResponse> ExitQuizAsync(Guid studentQuizId, Guid studentId)
        => await CompleteQuizAsync(studentQuizId, null, nameof(StudentQuizStatus.Exited), studentId);


    public async Task<QuizApiResponse> AutoSubmitQuizAsync(Guid studentQuizId, Guid studentId)
        => await CompleteQuizAsync(studentQuizId, DateTime.UtcNow, nameof(StudentQuizStatus.AutoSubmitted), studentId);

    private async Task<QuizApiResponse> CompleteQuizAsync(Guid studentQuizId, DateTime? completedOn,string status, Guid studentId)
    {
        var studentQuiz = await _context.StudentQuizzes
            .AsTracking()
            .FirstOrDefaultAsync(s => s.Id == studentQuizId);

        if (studentQuiz == null)
        {
            return QuizApiResponse.Fail("Quiz does not exist");
        }

        if (studentQuiz.StudentId != studentId)
        {
            return QuizApiResponse.Fail("Unauthorized access");
        }


        if (studentQuiz.CompletedOn.HasValue || studentQuiz.Status == nameof(StudentQuizStatus.Exited))
        {
            return QuizApiResponse.Fail("Quiz already completed");
        }

        try
        {
            studentQuiz.CompletedOn = completedOn;
            studentQuiz.Status = status;

            var studentQuizQuestions = await _context.StudentQuizQuestion
                .Where(s => s.StudentQuizId == studentQuizId)
                .ToArrayAsync();

            _context.StudentQuizQuestion.RemoveRange(studentQuizQuestions);
            await _context.SaveChangesAsync();

            return QuizApiResponse.Success();

        }
        catch (Exception e)
        {
            return QuizApiResponse.Fail(e.Message);
        }

    }

}


