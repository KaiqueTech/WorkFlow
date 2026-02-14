using WorkFlow.Domain.Models;

namespace WorkFlow.Domain.Interfaces
{
    public interface IRequestRepository
    {
        Task<RequestModel?> GetByIdAsync(Guid id);
        Task AddAsync(RequestModel request);
        Task UpdateAsync(Guid requestId,RequestModel request);
    }
}
