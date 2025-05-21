using System;

namespace FooDOC.api.Models.DTOs;

// DTO för inloggning
public class LoginDTO
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
