using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Auth.Api.Data;
using Auth.Api.Dto;
using Auth.Api.Entities;
using Auth.Api.Events;
using Auth.Api.Settings;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NJsonSchema;
using SchemaRegistry.Schemas.Users.Created;

namespace Auth.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserCreatedEventSchemaRegistry _schemaRegistry;
    private readonly JwtSettings _jwtSettings;
    private readonly KafkaSettings _kafkaSettings;

    public AuthController(IOptions<JwtSettings> jwtSettings, IOptions<KafkaSettings> kafkaSettings, IUserCreatedEventSchemaRegistry schemaRegistry)
    {
        _schemaRegistry = schemaRegistry;
        _kafkaSettings = kafkaSettings.Value;
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
            PublicId = Guid.NewGuid(),
            Email = userRegister.Email,
            Password = userRegister.Password,
            Role = userRegister.Role
        };

        db.Users.Add(user);
        db.SaveChanges();

        ProduceUserCreatedEvent(user);

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
        var audience = _jwtSettings.Audience;
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Role", user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),

            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }

    private void ProduceUserCreatedEvent(User user)
    {
        var producerConfig = new ProducerConfig { BootstrapServers = _kafkaSettings.BootstrapServers };
        var producer = new ProducerBuilder<string, string>(producerConfig).Build();

        var data = JsonSerializer.Serialize(new UserCreatedEvent(user.PublicId, user.Name, user.Role, user.Email)
        {
            EventMeta = new EventMeta<UserCreatedEventVersion>
            {
                Id = Guid.NewGuid(),
                Producer = "Auth",
                Name = "UserCreatedEvent",
                Time = DateTime.UtcNow,
                Version = UserCreatedEventVersion.V1
            }
        });

        var jsonSchema = _schemaRegistry.GetSchemaByVersion(UserCreatedEventVersion.V1.ToString()).Result;
        var validationErrors = JsonSchema.FromJsonAsync(jsonSchema).Result.Validate(data);

        if (validationErrors.Any())
        {
            throw new InvalidOperationException("Invalid format of event");
        }
        
        producer.Produce("users-stream", new Message<string, string>
        {
            Key = user.Id.ToString(), 
            Value = data
        });
    }
}