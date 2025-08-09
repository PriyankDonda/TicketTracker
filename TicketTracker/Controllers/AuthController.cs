using System.Net;
using Microsoft.AspNetCore.Mvc;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Services;

namespace TicketTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            Console.WriteLine("registering user.....");
            await _authService.Register(dto);
            return ApiResponseHelper.Success("User registered successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error happend: "+ ex.Message);
            return ApiResponseHelper.Error(ex.Message, statusCode: HttpStatusCode.BadRequest);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var result = await _authService.Login(dto);
            return ApiResponseHelper.Success(result);
        }
        catch (Exception e)
        {
            return ApiResponseHelper.Error(e.Message, statusCode: HttpStatusCode.Unauthorized);
        }
    }
}