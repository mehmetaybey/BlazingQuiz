using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Shared.DTOs;

public class CategoryDto
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
}