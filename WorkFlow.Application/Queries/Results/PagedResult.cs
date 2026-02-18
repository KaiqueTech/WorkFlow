namespace WorkFlow.Application.Queries.Results
{
    public record PagedResult<T>(IEnumerable<T> Items, int TotalCount);
}
