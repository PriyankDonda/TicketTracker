using TicketTracker.DTOs;
using TicketTracker.Models;

namespace TicketTracker.Services;

public interface IAuthService
{
    Task Register(RegisterDto dto);
    Task<AuthResponseDto> Login(LoginDto dto);
    Task<string> GenerateJwtToken(User user);
}