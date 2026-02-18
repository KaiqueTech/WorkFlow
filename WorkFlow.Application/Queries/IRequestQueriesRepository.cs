using WorkFlow.Application.DTOs;
using WorkFlow.Application.Queries.Filters;
using WorkFlow.Application.Queries.Results;

namespace WorkFlow.Application.Queries
{
    public interface IRequestQueriesRepository
    {
        Task<PagedResult<RequestListingDto>> GetFilteredRequestsAsync(RequestFilter filter, string userId, string role);

        Task<RequestDetailDto?> GetDetailByIdAsync(Guid requestId);

        Task<IEnumerable<RequestHistoryDto>> GetRequestHistoryAsync(Guid requestId, string userId, string role);
    }
}
