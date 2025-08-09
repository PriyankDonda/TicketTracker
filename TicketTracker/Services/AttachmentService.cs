using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketTracker.Data;
using TicketTracker.DTOs;
using TicketTracker.Models;

namespace TicketTracker.Services;

public class AttachmentService : IAttachmentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public AttachmentService(ICurrentUserService currentUserService, AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    
    public async Task<IEnumerable<AttachmentResponseDto>> CreateAsync(int ticketId, AttachmentRequestDto request)
    { 
        var attachments = new List<Attachment>();
        if (request.Urls.Any())
        {
            attachments = request.Urls.Select(url => new Attachment()
                {
                    TicketId = ticketId,
                    Url = url,
                    CreatedAt = DateTime.UtcNow
                }
            ).ToList();
            
            await _context.Attachments.AddRangeAsync(attachments);
            await _context.SaveChangesAsync();
        }
        
        return _mapper.Map<IEnumerable<AttachmentResponseDto>>(attachments);
    }

    public async Task<IEnumerable<AttachmentResponseDto>> GetListAsync(int ticketId)
    {
        var attachmets = await _context.Attachments.Where(a => a.TicketId == ticketId).ToListAsync();
        
        return _mapper.Map<IEnumerable<AttachmentResponseDto>>(attachmets);
    }
}