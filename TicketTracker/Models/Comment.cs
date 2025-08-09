namespace TicketTracker.Models;

public class Comment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    
    public string Message { get; set; }
    
    public DateTime CreatedAt { get; set; }
}