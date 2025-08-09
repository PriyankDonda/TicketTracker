using TicketTracker.DTOs;
using TicketTracker.Helpers;

namespace TicketTracker.Services;

public interface ITicketService
{
    Task<TicketResponseDto> GetByIdAsync(int id);
    Task<PaginatedResult<TicketResponseDto>> GetListAsync(int page = 1, int pageSize = 10, TicketFilterRequestDto filter = null);
    Task<TicketResponseDto> CreateAsync(TicketRequestDto ticketRequestDto);
    Task<bool> UpdateTicketStatusAsync(int id, string status);
    Task<bool> UpdateAssignedToAsync(int id, int assignedToId);
    Task<bool> DeleteAsync(int id);
    
    
}