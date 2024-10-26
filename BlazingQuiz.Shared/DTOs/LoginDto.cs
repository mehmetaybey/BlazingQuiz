using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class LoginDto
{
    [Required,EmailAddress,DataType(DataType.EmailAddress)]
    public string UserName { get; set; }
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
    
}