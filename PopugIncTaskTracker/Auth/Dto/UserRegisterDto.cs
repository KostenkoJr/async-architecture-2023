using Auth.Api.Entities;

namespace Auth.Api.Dto;

public record UserRegisterDto(string Password, string Email, string Name, Role Role);