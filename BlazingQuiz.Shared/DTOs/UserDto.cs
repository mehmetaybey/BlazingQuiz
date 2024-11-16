namespace BlazingQuiz.Shared.DTOs;

//public record UserDto(Guid Id,string Name,string Email,string Phone,bool IsApproved);

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsApproved { get; set; }

    public UserDto(Guid id, string name, string email, string phone, bool isApproved)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        IsApproved = isApproved;
    }

}