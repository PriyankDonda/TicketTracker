using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Services;

namespace TicketTracker.Controllers;

[Authorize]
[ApiController]
[Route("api/Ticket/{ticketId}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int ticketId, [FromBody] CommentRequestDto requestDto)
    {
        var comment = await _commentService.CreateAsync(ticketId, requestDto);
        
        return ApiResponseHelper.Success(comment);
    }

    [HttpGet]
    public async Task<IActionResult> GetList(int ticketId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var comments = await _commentService.GetListAsync(ticketId, page, pageSize);
        
        return ApiResponseHelper.Success(comments);
    }
}