using WorkFlow.Application.DTOs;

namespace WorkFlow.Application.Queries
{
    public interface IRequestQueriesRepository
    {
        Task<PagedResult<RequestListingDto>> GetFilteredRequestsAsync(RequestFilter filter, string userId, string role);

        // Detalhes para a Timeline (Completa)
        Task<RequestDetailDto?> GetDetailByIdAsync(Guid requestId);

        Task<IEnumerable<RequestHistoryDto>> GetRequestHistoryAsync(Guid requestId, string userId, string role);
    }
}
