namespace Shared.Parameters;

public class PagedList<T>
{
    public List<T>    Items    { get; set; }
    public MetaData   MetaData { get; set; }

    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        MetaData = new MetaData
        {
            TotalCount  = count,
            PageSize    = pageSize,
            CurrentPage = pageNumber,
            TotalPages  = (int)Math.Ceiling(count / (double)pageSize)
        };
    }
}