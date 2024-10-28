using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Api.Data.Repositories;
using BlazingQuiz.Shared;
using BlazingQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BlazingQuiz.Api.Services;

public class AuthService
{
    private readonly QuizContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;

    public AuthService(QuizContext context,IPasswordHasher<User> passwordHasher,IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == loginDto.UserName);
        if (user==null)
        {//invalid username
            return new AuthResponseDto(default,"Invalid username");
        }

        var passwordResult= _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (passwordResult==PasswordVerificationResult.Failed)
        {//invalid password
            return new AuthResponseDto(default,"Incorrect password");
        }
        //Generate JWT
        var jwt = GenerateJwtToken(user);
        var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Role, jwt);
        return new AuthResponseDto(loggedInUser);
    }

    private  string GenerateJwtToken(User user)
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.Name),
            new Claim(ClaimTypes.Role,user.Role),
           
        ];
        var secretKey = _configuration.GetValue<string>("Jwt:Secret");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
        var signingCred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer:_configuration.GetValue<string>("Jwt:Issuer"),
            audience:_configuration.GetValue<string>("Jwt:Audience"),
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireInMinutes")),
            signingCredentials:signingCred
            );
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }
}