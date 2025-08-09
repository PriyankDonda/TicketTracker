using System.ComponentModel.DataAnnotations;

namespace TicketTracker.DTOs;

public class TicketRequestDto
{
    [Required, MaxLength(300)]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required,AllowedValues(["Low", "Medium", "High", "Critical"])]
    public string Priority { get; set; }
}

public class TicketResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }    
    public string Status { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedById { get; set; }
    public int? AssignedToId { get; set; }
    
    public UserDto CreatedBy { get; set; }
    public UserDto AssignedTo { get; set; }
    
}

public class TicketFilterRequestDto
{
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public DateTime? UpdatedFrom { get; set; }
    public DateTime? UpdatedTo { get; set; }
    public string? Title { get; set; }
    public List<string> Priority { get; set; }
    public List<string> Status { get; set; }
}