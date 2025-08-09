using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Services;

namespace TicketTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/Ticket/{ticketId}/[controller]")]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, AttachmentRequestDto request)
    {
        var attachments = await _attachmentService.CreateAsync(ticketId, request);
        
        return ApiResponseHelper.Success(attachments);
    }

    [HttpGet]
    public async Task<IActionResult> Get(int ticketId)
    {
        var attachments = await _attachmentService.GetListAsync(ticketId);
        return ApiResponseHelper.Success(attachments);
    }
}