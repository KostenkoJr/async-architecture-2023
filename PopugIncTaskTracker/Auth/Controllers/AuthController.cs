using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Api.Data;
using Auth.Api.Dto;
using Auth.Api.Models;
using Auth.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    public AuthController(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    public string Register(UserRegisterDto userRegister)
    {
        using var db = new AuthDbContext();
        
        var isUserAlreadyExists = db.Users.FirstOrDefault(u => u.Email == userRegister.Email) is not null;
        if (isUserAlreadyExists)
        {
            throw new InvalidOperationException($"User with email {userRegister.Email} already exists");
        }

        var user = new User
        {
            Name = userRegister.Name,
            Email = userRegister.Email,
            Password = userRegister.Password,
            Role = userRegister.Role
        };

        db.Users.Add(user);
        db.SaveChanges();

        var token = GenerateToken(user);
        return token;
    }
    
    [HttpPost("login")]
    public string GetLogin(UserLoginDto userLogin)
    {
        using var db = new AuthDbContext();
        var user = db.Users.FirstOrDefault(u => u.Email == userLogin.Email && u.Password == userLogin.Password);
        if (user is null)
        {
            throw new InvalidOperationException("Invalid email or password");
        }

        var token = GenerateToken(user);
        return token;
    }

    private string GenerateToken(User user)
    {
        var issuer = _jwtSettings.Issuer;
        var audience =  _jwtSettings.Audience;
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim("Role", user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
}