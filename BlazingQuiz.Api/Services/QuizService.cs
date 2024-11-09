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
            var quizId = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
            Quiz dbQuiz;

            if (dto.Id == Guid.Empty)
            {
                // Yeni bir Quiz oluşturulacaksa
                dbQuiz = new Quiz
                {
                    Id = quizId,
                    Name = dto.Name,
                    CategoryId = dto.CategoryId,
                    TotalQuestions = dto.TotalQuestions,
                    TimeInMinutes = dto.TimeInMinutes,
                    IsActive = dto.IsActive
                };

                foreach (var questionDto in dto.Question)
                {
                    var newQuestion = new Questions
                    {
                        Id = Guid.NewGuid(),
                        Text = questionDto.Text,
                        Options = questionDto.Options.Select(o => new Options
                        {
                            Id = 0, // IDENTITY alanı olduğundan 0 veriyoruz
                            Text = o.Text,
                            IsCorrect = o.IsCorrect
                        }).ToList()
                    };

                    dbQuiz.Questions.Add(newQuestion); // Soruları doğrudan koleksiyona ekle
                }
                _context.Quizzes.Add(dbQuiz);
            }
            else
            {
                // Var olan bir Quiz güncelleniyorsa
                dbQuiz = await _context.Quizzes
                        .Include(q => q.Questions)
                        .ThenInclude(q => q.Options)
                         .FirstOrDefaultAsync(q => q.Id == quizId);

                if (dbQuiz == null)
                {
                    return QuizApiResponse.Fail("Quiz doesn't exist");
                }

                // Güncelleme işlemleri
                dbQuiz.CategoryId = dto.CategoryId;
                dbQuiz.IsActive = dto.IsActive;
                dbQuiz.Name = dto.Name;
                dbQuiz.TimeInMinutes = dto.TimeInMinutes;
                dbQuiz.TotalQuestions = dto.TotalQuestions;

                // Var olan soruları ve seçenekleri güncelle
                foreach (var questionDto in dto.Question)
                {
                    var dbQuestion = dbQuiz.Questions.FirstOrDefault(q => q.Id == questionDto.Id);
                    if (dbQuestion != null)
                    {
                        dbQuestion.Text = questionDto.Text;

                        // Var olan seçenekleri güncelle veya ekle
                        foreach (var optionDto in questionDto.Options)
                        {
                            var dbOption = dbQuestion.Options.FirstOrDefault(o => o.Id == optionDto.Id);
                            if (dbOption != null)
                            {
                                dbOption.Text = optionDto.Text;
                                dbOption.IsCorrect = optionDto.IsCorrect;
                            }
                            else
                            {
                                dbQuestion.Options.Add(new Options
                                {
                                    Text = optionDto.Text,
                                    IsCorrect = optionDto.IsCorrect
                                });
                            }
                        }
                    }
                    else
                    {
                        // Yeni bir soru ekle
                        var newQuestion = new Questions
                        {
                            Text = questionDto.Text,
                            Options = questionDto.Options.Select(o => new Options
                            {
                                Text = o.Text,
                                IsCorrect = o.IsCorrect
                            }).ToList()
                        };
                        dbQuiz.Questions.Add(newQuestion); // Soruları doğrudan koleksiyona ekle
                    }
                }
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

        public async Task<QuizListDto[]> GetQuizesAsync()
        {
            return await _context.Quizzes.Select(q => new QuizListDto
            {
                Id = q.Id,
                Name = q.Name ?? string.Empty,
                TimeInMinutes = q.TimeInMinutes,
                TotalQuestions = q.TotalQuestions,
                IsActive = q.IsActive,
                CategoryId = q.CategoryId,
                CategoryName = q.Category != null ? q.Category.Name ?? string.Empty : string.Empty
            }).ToArrayAsync();
        }

        public async Task<QuestionDto[]> GetQuizQuestionsAsync(Guid quizId)
            => await _context.Questions.Where(q => q.QuizId == quizId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text
                }).ToArrayAsync();

        public async Task<QuizSaveDto?> GetQuizToEditAsync(Guid quizId)
        {
            var quiz = await _context.Quizzes
                .Where(q => q.Id == quizId)
                .Select(qz => new QuizSaveDto
                {
                    Id = qz.Id,
                    CategoryId = qz.CategoryId,
                    IsActive = qz.IsActive,
                    Name = qz.Name ?? string.Empty, // Provide a default value if qz.Name is null
                    TimeInMinutes = qz.TimeInMinutes,
                    TotalQuestions = qz.TotalQuestions,
                    Question = qz.Questions
                        .Select(q => new QuestionDto
                        {
                            Id = q.Id,
                            Text = q.Text,
                            Options = q.Options.Select(o => new OptionDto
                            {
                                Text = o.Text,
                                Id = o.Id, // Use the existing Id
                                IsCorrect = o.IsCorrect
                            }).ToList()
                        }).ToList()
                }).FirstOrDefaultAsync();

            return quiz;//quizId comes without problems
        }


    }
}
