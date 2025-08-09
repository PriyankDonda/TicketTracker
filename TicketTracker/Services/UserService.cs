using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketTracker.Data;
using TicketTracker.DTOs;
using TicketTracker.Helpers;

namespace TicketTracker.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public UserService(AppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<UserDto> GetCurrentUser()
    {
        var user = await _context.Users.FindAsync(_currentUserService.UserId);
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetListAsync(int page = 1, int pageSize = 10)
    {
        var users = await _context.Users.OrderByDescending(t => t.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<bool> UpdateAsync(UpdateUserDto updateUserDto)
    {
        if(_currentUserService.UserId != updateUserDto.Id)
            return false;
        
        var user = await _context.Users.FindAsync(updateUserDto.Id);
        
        if(user == null)
            return false;
        
        _mapper.Map(updateUserDto, user);
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePasswordAsync(UserChangePasswordDto dto)
    {
        if(_currentUserService.UserId != dto.Id)
            return false;
        
        var user = await _context.Users.FindAsync(dto.Id);
        
        if(user == null || !BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
            return false;

        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        return await _context.SaveChangesAsync() > 0;
    }


    public async Task<bool> UpdateUserRoleAsync(int id, string role)
    {
        var user = await _context.Users.FindAsync(id);
        
        if (user == null || !Enum.TryParse<ROLE>(role, out var roleEnum))
            return false;
        
        user.Role = roleEnum.ToString();
        
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }
}