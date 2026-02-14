using WorkFlow.Domain.Enuns;

namespace WorkFlow.Application.DTOs
{
    public record CreateRequestDto(
        string Title,
        string Description,
        RequestCategoryEnum Category,
        RequestPriorityEnum Priority
    );
}
