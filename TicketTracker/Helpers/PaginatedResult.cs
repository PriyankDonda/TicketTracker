namespace TicketTracker.Helpers;

public class PaginatedResult<T>
{
    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int Count { get; }

    public PaginatedResult(IEnumerable<T> items, int page, int pageSize, int count)
    {
        Items = items.ToList();
        Page = page;
        PageSize = pageSize;
        Count = count;
    }
}