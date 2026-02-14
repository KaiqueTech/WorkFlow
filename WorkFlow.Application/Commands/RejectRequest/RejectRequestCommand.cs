namespace WorkFlow.Application.Commands.RejectRequest
{
    public record RejectRequestCommand(Guid RequestId,string ManagerId,string UserRole,string? Comment);
}
