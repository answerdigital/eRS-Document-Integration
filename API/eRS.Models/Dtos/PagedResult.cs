namespace eRS.Models.Dtos;

public abstract class PagedResultBase
{
    public int CurrentPage { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public int RowCount { get; set; }
}

[Serializable]
public sealed class PagedResult<T> : PagedResultBase where T : class
{
    public IList<T> Results { get; set; }

    public PagedResult()
    {
        this.Results = new List<T>();
    }
}