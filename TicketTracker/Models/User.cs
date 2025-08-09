namespace TicketTracker.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public IEnumerable<Ticket> CreatedTickets { get; set; }
    public IEnumerable<Ticket> AssignedTickets { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
}