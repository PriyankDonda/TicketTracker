namespace TicketTracker.Models;

public class Attachment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
    
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }
    
    
}