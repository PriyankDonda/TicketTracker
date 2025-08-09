using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketTracker.Data;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Models;

namespace TicketTracker.Services;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CommentService(AppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<CommentResponseDto> CreateAsync(int ticketId, CommentRequestDto requestDto)
    {
        var comment = _mapper.Map<Comment>(requestDto);
        comment.TicketId = ticketId;
        comment.CreatedById = _currentUserService.UserId;
        comment.CreatedAt = DateTime.UtcNow;
        
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<CommentResponseDto>(comment);
    }

    public async Task<PaginatedResult<CommentResponseDto>> GetListAsync(int ticketId, int page = 1, int pageSize = 10)
    {
        var query = _context.Comments
            .Where(c => c.TicketId == ticketId)
            .AsQueryable();
        
        var count = await query.CountAsync();

        var comments = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.CreatedBy)
            .ToListAsync();
        
        var commentResponseDtos = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
        
        return new PaginatedResult<CommentResponseDto>(commentResponseDtos, page, pageSize, count);
    }
}