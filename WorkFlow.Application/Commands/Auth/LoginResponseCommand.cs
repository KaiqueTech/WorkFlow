namespace WorkFlow.Application.Commands.Auth
{
    public record LoginResponseCommand(bool Success, string Message,string? Token = null!);
}
