namespace TicketsGO.Services;

public class PaginatedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    // Calcula el total de páginas automáticamente
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}