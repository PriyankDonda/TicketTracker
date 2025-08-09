using System.ComponentModel.DataAnnotations;

namespace TicketTracker.DTOs;

public class AttachmentRequestDto
{
    [Required]
    public List<string> Urls { get; set; }
}

public class AttachmentResponseDto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }
}