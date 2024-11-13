using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class RegisterDto
{   
    [Required,MaxLength(20)]
    public string Name { get; set; }
    [Required,EmailAddress,DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    
    [Length(10, 50)]
    public string? Phone { get; set; }

    [MaxLength(250)]
    [DataType(DataType.Password)]
    [Required]
    public string? Password { get; set; }
}