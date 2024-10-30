using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazingQuiz.Api.Services;

public class CategoryService
{
    private readonly QuizContext _context;

    public CategoryService(QuizContext context)
    {
        _context = context;
    }

    public async Task<QuizApiResponse> SaveCategoryAsync(CategoryDto categoryDto)
    {
        if (await _context.Categories
                .AsNoTracking()
                .AnyAsync(c=>c.Name==categoryDto.Name && c.Id !=categoryDto.Id))
        {
            return QuizApiResponse.Fail("Category with same name exists already");
        }
        
        if (categoryDto.Id==Guid.Empty)
        {
            //create new category
            var category = new Category
            {
                Name = categoryDto.Name
            };
            _context.Categories.Add(category);
        }
        else
        {
            //edit or update
            var dbCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryDto.Id);
            if (dbCategory==null)
            {
                return QuizApiResponse.Fail("Category does not exist");
            }

            dbCategory.Name = categoryDto.Name;
            _context.Categories.Update(dbCategory);
        }

        await _context.SaveChangesAsync();
        return QuizApiResponse.Success();
    }

    public async Task<CategoryDto[]> GetCategoriesAsync() =>
        await _context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToArrayAsync();
}