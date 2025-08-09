namespace TicketTracker.Helpers;

public enum ROLE
{
    Client,
    Admin,
    Agent
}

public enum TICKETSTATUS
{
    Open = 1,
    InProgress = 2,
    Resolved = 3,
    Closed = 4,
    Rejected = 5
}

public enum TICKETPRIORITY
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}