using System.ComponentModel.DataAnnotations;

namespace TicketTracker.DTOs;

public class RegisterDto
{
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MinLength(8)]
    public string Password { get; set; }
}

public class LoginDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
}