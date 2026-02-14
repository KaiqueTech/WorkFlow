using WorkFlow.Application.Exceptions;
using WorkFlow.Domain.Interfaces;

namespace WorkFlow.Application.Commands.RejectRequest
{
    public class RejectionRequestCommandHandler(IRequestRepository repository)
    {
        public async Task Handle(RejectRequestCommand command)
        {
            var request = await repository.GetByIdAsync(command.RequestId);
            if (request == null) throw new NotFoundException("request not found");

            if (command.UserRole != "Manager")
            {
                throw new UnauthorizedAccessException("Apenas gestores podem aprovar solicitações.");
            }

            request.Reject(command.ManagerId, command.Comment);

            await repository.UpdateAsync(request.Id, request);
        }
    }
}
