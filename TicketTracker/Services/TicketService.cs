using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketTracker.Data;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Models;

namespace TicketTracker.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public TicketService(AppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<TicketResponseDto> GetByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.CreatedBy)
            .Include((t => t.AssignedTo))
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (ticket == null)
            return null;
        
        return _mapper.Map<TicketResponseDto>(ticket);
    }

    public async Task<PaginatedResult<TicketResponseDto>> GetListAsync(int page = 1, int pageSize = 10, TicketFilterRequestDto filter = null)
    {
        var query = _context.Tickets.AsQueryable();

        //based on role fetch tickets
        if (_currentUserService.Role == ROLE.Client.ToString())
        {
            query = query.Where(x => x.CreatedById == _currentUserService.UserId);
        }
        else if (_currentUserService.Role == ROLE.Agent.ToString())
        {
            query = query.Where(x => x.AssignedToId == _currentUserService.UserId);
        }
        
        // Filtering
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            query = query.Where(t => t.Title.Contains(filter.Title));
        }

        if (filter.Priority.Any())
        {
            query = query.Where(t => filter.Priority.Contains(t.Priority));
        }

        if (filter.Status.Any())
        {
            query = query.Where(t => filter.Status.Contains(t.Status));
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(t => t.CreatedAt >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(t => t.CreatedAt <= filter.CreatedTo.Value);
        }

        if (filter.UpdatedFrom.HasValue)
        {
            query = query.Where(t => t.UpdatedAt >= filter.UpdatedFrom.Value);
        }

        if (filter.UpdatedTo.HasValue)
        {
            query = query.Where(t => t.UpdatedAt <= filter.UpdatedTo.Value);
        }
        
        var count = await query.CountAsync();

        var tickets = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(t => t.CreatedBy)
            .Include((t => t.AssignedTo))
            .ToListAsync();
        
        
        var ticketResponseDtos = _mapper.Map<IEnumerable<TicketResponseDto>>(tickets);
        
        return new PaginatedResult<TicketResponseDto>(ticketResponseDtos, page, pageSize, count);
    }

    public async Task<TicketResponseDto> CreateAsync(TicketRequestDto ticketRequestDto)
    {
        var ticket = _mapper.Map<Ticket>(ticketRequestDto);
        ticket.CreatedById = _currentUserService.UserId;
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.Status = TICKETSTATUS.Open.ToString();
        
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<TicketResponseDto>(ticket);
    }

    public async Task<bool> UpdateTicketStatusAsync(int id, string status)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null || !Enum.TryParse<TICKETSTATUS>(status, out var statusEnum))
            return false;
        
        ticket.Status = statusEnum.ToString();
        ticket.UpdatedAt = DateTime.UtcNow;
        
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> UpdateAssignedToAsync(int id, int assignedToId)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        var assignedTo = await _context.Users.FindAsync(assignedToId);

        if (ticket == null || assignedTo == null || assignedTo.Role == ROLE.Client.ToString())
        {
            return false;
        }
        
        ticket.AssignedToId = assignedToId;
        ticket.UpdatedAt = DateTime.UtcNow;
        _context.Tickets.Update(ticket);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        
        if(ticket == null)
            return false;
        
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        
        return true;
    }
}