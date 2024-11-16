using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;

namespace BlazingQuiz.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Admin)));

        group.MapGet("", async (UserApprovedFilter approvedType, int startIndex, int pageSize, UserService service)=>
        {
            //var approvedFilter=Enum.Parse<UserApprovedFilter>(approveType, true);
            return Results.Ok(await service.GetUsersAsync(approvedType, startIndex, pageSize));
        });

        group.MapPatch("{userId:Guid}/toogle-status", async (Guid userId, UserService service)
            =>
        {
            await service.ToggleUserApprovedStatusAsync(userId);
            return Results.Ok();
        }); 
        return app;
    }
}