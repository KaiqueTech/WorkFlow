using WorkFlow.Domain.Enuns;

namespace WorkFlow.Application.Queries.Filters
{
    public record RequestFilter(string? SearchText, RequestStatusEnum? Status, RequestPriorityEnum? Priority, RequestCategoryEnum? Category, int Page = 1, int PageSize = 10);
}
