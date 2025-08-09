using TicketTracker.Helpers;

namespace TicketTracker.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }    // Low, Medium, High
    public string Status { get; set; } = TICKETSTATUS.Open.ToString();    // Open, In Progress, Resolved, Rejected
    
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    
    public int? AssignedToId { get; set; }
    public User AssignedTo { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public IEnumerable<Comment> Comments { get; set; }
    public IEnumerable<Attachment> Attachments { get; set; }
}