using WorkFlow.Domain.Enuns;

namespace WorkFlow.Application.Commands.CreateRequest
{
    public record CreateRequestCommand(string Title,string Description,RequestCategoryEnum Category,RequestPriorityEnum Priority,string UserId);
}
