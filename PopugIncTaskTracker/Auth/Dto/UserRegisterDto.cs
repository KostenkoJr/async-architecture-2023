using Auth.Api.Models;

namespace Auth.Api.Dto;

public record UserRegisterDto(string Password, string Email, string Name, Role Role);