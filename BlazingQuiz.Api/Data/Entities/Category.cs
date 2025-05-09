using System.ComponentModel.DataAnnotations;

namespace BlazingQuiz.Api.Data.Entities;

public class Category
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }
}