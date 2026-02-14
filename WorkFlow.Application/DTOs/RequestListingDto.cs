using WorkFlow.Domain.Enuns;

namespace WorkFlow.Application.DTOs
{
    public record RequestListingDto(
        Guid Id,
        string Title,
        string Description,
        RequestCategoryEnum Category,
        RequestPriorityEnum Priority,
        RequestStatusEnum Status,
        DateTime CreatedAt);
}
