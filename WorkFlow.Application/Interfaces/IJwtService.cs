using WorkFlow.Domain.Models;

namespace WorkFlow.Application.Interfaces
{
    public interface IJwtService
    {

        Task<string> GenerateToken(ApplicationUserModel user);
    }
}
