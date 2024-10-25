using System.ComponentModel.DataAnnotations;
using BlazingQuiz.Shared;

namespace BlazingQuiz.Api.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(20)]
    public string? Name { get; set; } 
    [EmailAddress]
    public string? Email { get; set; }
    [Length(10,50)]
    public string? Phone { get; set; }
    
    [MaxLength(250)]
    public string? PasswordHash { get; set; }
    
    [MaxLength(15)]
    public string Role { get; set; } = nameof(UserRole.Student);
}