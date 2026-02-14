using WorkFlow.Domain.Interfaces;
using WorkFlow.Domain.Models;

namespace WorkFlow.Application.Commands.CreateRequest
{
    public class CreateRequestCommandHandler(IRequestRepository repo)
    {
        public async Task HandleAsync(CreateRequestCommand command)
        {
            var request = RequestModel.CreateRequest(
                command.Title,
                command.Description,
                command.Category,
                command.Priority,
                command.UserId);

            await repo.AddAsync(request);
        }
    }
}
