using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared;

namespace BlazingQuiz.Api.Endpoints;

public static class AdminEndpoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/home-data", async (AdminService service) =>
            Results.Ok(await service.GetHomeDataAsync()))    
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Admin)));
        
        
        var group = app.MapGroup("/api/users")
            .RequireAuthorization(p=>p.RequireRole(nameof(UserRole.Admin)));

        group.MapGet("", async (UserApprovedFilter approvedType, int startIndex, int pageSize, AdminService service)=>
        {
            //var approvedFilter=Enum.Parse<UserApprovedFilter>(approveType, true);
            return Results.Ok(await service.GetUsersAsync(approvedType, startIndex, pageSize));
        });

        group.MapPatch("{userId:Guid}/toggle-status", async (Guid userId, AdminService service)
            =>
        {
            await service.ToggleUserApprovedStatusAsync(userId);
            return Results.Ok();
        }); 
        return app;
    }
}