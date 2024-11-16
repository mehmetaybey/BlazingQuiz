using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;

namespace BlazingQuiz.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Admin)));

        group.MapGet("", async (UserApprovedFilter filter, int pageNumber, int pageSize, UserService service)
            => Results.Ok(await service.GetUsersAsync(filter,pageNumber,pageSize)));
        group.MapPatch("{userId:Guid}/toogle-status", async (Guid userId, UserService service)
            =>
        {
            await service.ToggleUserApprovedStatusAsync(userId);
            return Results.Ok();
        });
        return app;
    }
}