using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services;

public class AdminService
{
    private readonly IDbContextFactory<QuizContext> _contextFactory;

    public AdminService(IDbContextFactory<QuizContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    public async Task<AdminHomeDataDto> GetHomeDataAsync()
    {
        var totalCategoriesTask=  _contextFactory.CreateDbContext().Categories.CountAsync();
        var totalStudentsTask=  _contextFactory.CreateDbContext().Users.Where(u=>u.Role==nameof(UserRole.Student)).CountAsync();
        var approvedStudentsTask = _contextFactory.CreateDbContext().Users.Where(u => u.IsApproved && u.Role == nameof(UserRole.Student)).CountAsync();
        var totalQuizzesTask=  _contextFactory.CreateDbContext().Quizzes.CountAsync();
        var activeQuizzesTask=  _contextFactory.CreateDbContext().Quizzes.Where(q=>q.IsActive).CountAsync();

        var totalCategories = await totalCategoriesTask;
        var totalStudents = await totalStudentsTask;
        var approvedStudents = await approvedStudentsTask;
        var totalQuizzes = await totalQuizzesTask;
        var activeQuizzes = await activeQuizzesTask;

        return new AdminHomeDataDto(totalCategories, totalStudents, approvedStudents, totalQuizzes, activeQuizzes);

    }
    public async Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter approvedType, int startIndex, int pageSize)
    {
        await using var context =_contextFactory.CreateDbContext();
        var query = context.Users.Where(u=>u.Role !=nameof(UserRole.Admin)).AsQueryable();
        if (approvedType != UserApprovedFilter.All)
        {
            if (approvedType == UserApprovedFilter.ApprovedOnly)
                query = query.Where(u => u.IsApproved);

            else
                query = query.Where(u => !u.IsApproved);
        }

        var total = await query.CountAsync();
        var users = await query.OrderByDescending(u => u.Id)
            .Skip(startIndex)
            .Take(pageSize)
            .Select(u=> new UserDto(u.Id,u.Name,u.Email,u.Phone,u.IsApproved))
            .ToArrayAsync();
        return new PagedResult<UserDto>(users, total);
    }

    public async Task ToggleUserApprovedStatusAsync(Guid userId)
    {
        using var context = _contextFactory.CreateDbContext();
        
        var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (dbUser !=null)
        {
            dbUser.IsApproved = !dbUser.IsApproved;
            await context.SaveChangesAsync();
        }
    }
    
}