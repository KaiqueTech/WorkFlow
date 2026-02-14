using WorkFlow.Domain.Exceptions;
using WorkFlow.Domain.Interfaces;

namespace WorkFlow.Application.Commands.ApproveRequest
{
    public class ApproveRequestCommandHandler(IRequestRepository repository)
    {

        public async Task Handle(ApproveRequestCommand command)
        {
            var request = await repository.GetByIdAsync(command.RequestId);
            if (request == null) throw new NotFoundException("request not found");

            
            if (command.UserRole != "Manager")
            {
                throw new UnauthorizedAccessException("Apenas gestores podem aprovar solicitações.");
            }

            request.Approve(command.ManagerId, command.Comment);

            await repository.UpdateAsync(request.Id, request);
        }
    }
}
