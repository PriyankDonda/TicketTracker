using System.ComponentModel.DataAnnotations;

namespace TicketTracker.DTOs;

public class CommentRequestDto
{
    [Required]
    public string Message { get; set; }
}

public class CommentResponseDto
{
    public int Id { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public UserDto CreatedBy { get; set; }
}