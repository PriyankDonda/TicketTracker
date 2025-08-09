using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketTracker.DTOs;
using TicketTracker.Helpers;
using TicketTracker.Models;
using TicketTracker.Services;

namespace TicketTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        
        if(ticket == null)
            return ApiResponseHelper.Error("Ticket not found", statusCode: HttpStatusCode.NotFound);
        
        return ApiResponseHelper.Success(ticket);
    }

    [HttpPost("Fetch")]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromBody] TicketFilterRequestDto dto = null)
    {
        var tickets = await _ticketService.GetListAsync(page, pageSize, dto);

        if (tickets == null)
        {
            return ApiResponseHelper.Error("Tickets not found", statusCode: HttpStatusCode.NotFound);
        }
        
        return ApiResponseHelper.Success(tickets);
    }

    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TicketRequestDto dto)
    {
        var ticket = await _ticketService.CreateAsync(dto);
        
        return ApiResponseHelper.Success(ticket);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/assign")]
    public async Task<IActionResult> AssignTicket(int id, [FromQuery] int assignedToId)
    {
        var updated = await _ticketService.UpdateAssignedToAsync(id, assignedToId);
        
        if (!updated)
        {
            return ApiResponseHelper.Error("Invalid Assign to or ticket not found", statusCode: HttpStatusCode.BadRequest);
        }

        return ApiResponseHelper.Success("Ticket Assigned To updated");
    }

    [Authorize(Roles = "Admin,Agent")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
    {
        var updated = _ticketService.UpdateTicketStatusAsync(id, status).Result;

        if (!updated)
        {
            return ApiResponseHelper.Error("Invalid status or ticket not found", statusCode: HttpStatusCode.BadRequest);
        }

        return ApiResponseHelper.Success("Ticket status updated");
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = _ticketService.DeleteAsync(id).Result;

        if (!deleted)
        {
            return ApiResponseHelper.Error("Ticket not found", statusCode: HttpStatusCode.NotFound);
        }
        
        return ApiResponseHelper.Success("Ticket deleted");
        // return NoContent();
    }
}