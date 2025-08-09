using TicketTracker.DTOs;
using TicketTracker.Helpers;

namespace TicketTracker.Services;

public interface ICommentService
{
    Task<CommentResponseDto> CreateAsync(int ticketId, CommentRequestDto requestDto);
    Task<PaginatedResult<CommentResponseDto>> GetListAsync(int ticketId, int page = 1, int pageSize = 10);
}