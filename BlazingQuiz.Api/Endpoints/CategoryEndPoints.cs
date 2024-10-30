using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints;

public static class CategoryEndPoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoint(this IEndpointRouteBuilder app)
    {
        var categoryGroup = app.MapGroup("/api/categories")
            .RequireAuthorization();//yetkilendirme sağladık

        categoryGroup.MapGet("",
            async (CategoryService categoryService) => Results.Ok(await categoryService.GetCategoriesAsync()));

        categoryGroup.MapPost("", async (CategoryDto dto, CategoryService service) =>
            Results.Ok(await service.SaveCategoryAsync(dto)))
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Admin)));
        return app;
    }
}