using TicketTracker.DTOs;

namespace TicketTracker.Services;

public interface IUserService
{
    Task<UserDto> GetCurrentUser();
    Task<List<UserDto>> GetListAsync(int page = 1, int pageSize = 10);
    Task<bool> UpdateAsync(UpdateUserDto updateUserDto);
    Task<bool> UpdatePasswordAsync(UserChangePasswordDto dto);
    Task<bool> UpdateUserRoleAsync(int id, string role);
}