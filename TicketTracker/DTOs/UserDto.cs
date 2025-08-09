using System.ComponentModel.DataAnnotations;
using TicketTracker.Services;

namespace TicketTracker.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UpdateUserDto
{
    [Required]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
}

public class UserChangePasswordDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string OldPassword { get; set; }
    [Required, MinLength(8)]
    public string NewPassword { get; set; }
}