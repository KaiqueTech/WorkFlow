namespace WorkFlow.Application.Commands.ApproveRequest
{
    public record ApproveRequestCommand(Guid RequestId,string ManagerId,string UserRole,string? Comment);
}
