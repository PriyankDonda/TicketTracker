using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketTracker.Data;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Models;

namespace TicketTracker.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthService(AppDbContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task Register(RegisterDto dto)
    {
        dto.Email = dto.Email.Trim().ToLower();
        
        if(await _context.Users.AnyAsync(x => x.Email.ToLower() == dto.Email))
            throw new ArgumentException("Email already exists");
        
        var user = _mapper.Map<User>(dto);
        user.Role = ROLE.Client.ToString();
        user.CreatedAt = DateTime.UtcNow;
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        
        var token = GenerateJwtToken(user).Result;

        var authResponseDto = new AuthResponseDto()
        {
            Token = token,
            Name = user.Name,
            Role = user.Role,
            Id = user.Id
        };
        
        return authResponseDto;
    }

    public async Task<string> GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var jwtConfig = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.GetValue<string>("Key")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtConfig.GetValue<string>("ExpiresInMinutes")));

        var token = new JwtSecurityToken(
            jwtConfig.GetValue<string>("Issuer"),
            jwtConfig.GetValue<string>("Audience"),
            claims,
            expires: expires,
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}