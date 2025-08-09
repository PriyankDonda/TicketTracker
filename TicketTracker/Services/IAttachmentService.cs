using TicketTracker.DTOs;

namespace TicketTracker.Services;

public interface IAttachmentService
{
    Task<IEnumerable<AttachmentResponseDto>> CreateAsync(int ticketId, AttachmentRequestDto request);
    Task<IEnumerable<AttachmentResponseDto>> GetListAsync(int ticketId);
}