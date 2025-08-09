using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Services;

namespace TicketTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Get()
    {
        var user = await _userService.GetCurrentUser();
        return ApiResponseHelper.Success(user);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        var updated = await _userService.UpdateAsync(dto);

        if (!updated)
        {
            return ApiResponseHelper.Error("User not found or trying to update other user profile",
                statusCode: HttpStatusCode.BadRequest);
        }
        
        return ApiResponseHelper.Success("User successfully updated");
    }

    [HttpPut("Password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UserChangePasswordDto dto)
    {
        //need to review this flow
        var updated = await _userService.UpdatePasswordAsync(dto);
        
        if (!updated)
        {
            return ApiResponseHelper.Error("Old Password not match or trying to update other user password or User not found.",
                statusCode: HttpStatusCode.BadRequest);
        }
        
        return ApiResponseHelper.Success("Password successfully updated");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Fetch")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var users = await _userService.GetListAsync(page, pageSize);
        return ApiResponseHelper.Success(users);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromQuery] string role)
    {
        var updated = await _userService.UpdateUserRoleAsync(id, role);
        
        if (!updated)
        {
            return ApiResponseHelper.Error("Invalid role or user not found", statusCode: HttpStatusCode.BadRequest);
        }

        return ApiResponseHelper.Success("User role updated");
    }
}