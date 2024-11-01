using BlazingQuiz.Shared.DTOs;
using Refit;

namespace BlazingQuiz.Web.Api
{
    [Headers("Authorization: Bearer ")]
    public interface ICategoryApi
    {
        [Post("/api/categories")]
        Task<QuizApiResponse> SaveCategoryAsync(CategoryDto dto);

        [Get("/api/categories")]
        Task<CategoryDto[]> GetCategoriesAsync();
    }
}
