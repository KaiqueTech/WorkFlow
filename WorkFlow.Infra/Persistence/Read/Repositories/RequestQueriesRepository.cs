using Dapper;
using Microsoft.AspNetCore.Connections;
using System.Text;
using WorkFlow.Application.DTOs;
using WorkFlow.Application.Interfaces;
using WorkFlow.Application.Queries;

namespace WorkFlow.Infra.Persistence.Read.Repositories
{
    public class RequestQueriesRepository(IDbConnectionFactory dbConnection) : IRequestQueriesRepository
    {
        public async Task<RequestDetailDto?> GetDetailByIdAsync(Guid requestId)
        {
            using var connection = dbConnection.CreateConnection();

            // 1. Busca a solicitação
            var sqlRequest = """
                                 SELECT 
                                 Id, Title, Description, 
                                 CAST(Status AS VARCHAR) AS Status, 
                                 CAST(Priority AS VARCHAR) AS Priority, 
                                 CAST(Category AS VARCHAR) AS Category, 
                                 CreatedByUserId AS CreatedByUser, 
                                 CreatedAt, UpdatedAt 
                                 FROM Requests 
                                 WHERE Id = @requestId
                            """;

            var request = await connection.QueryFirstOrDefaultAsync<RequestDetailDto>(sqlRequest, new { requestId });

            if (request == null) return null;

            // 2. Busca o histórico
            var sqlHistory = """
                                SELECT CAST(FromStatus AS VARCHAR) AS FromStatus, 
                                CAST(ToStatus AS VARCHAR) AS ToStatus, 
                                ChangedBy, ChangedAt, Comment 
                                FROM RequestHistories 
                                WHERE RequestId = @requestId 
                                ORDER BY ChangedAt DESC
                        """;

            var history = await connection.QueryAsync<RequestHistoryDto>(sqlHistory, new { requestId });

            request.History = history.ToList();

            return request;
        }

        public async Task<PagedResult<RequestListingDto>> GetFilteredRequestsAsync(RequestFilter filter, string userId, string role)
        {
            using var connection = dbConnection.CreateConnection();
            var parameters = new DynamicParameters();

            var filterSql = new StringBuilder(" WHERE 1=1 ");

            if (role != "Manager")
            {
                filterSql.Append(" AND CreatedByUserId = @userId ");
                parameters.Add("userId", userId);
            }

            if (filter.Status.HasValue)
            {
                filterSql.Append(" AND Status = @status ");
                parameters.Add("status", (int)filter.Status.Value);
            }

            if (filter.Priority.HasValue)
            {
                filterSql.Append(" AND Priority = @priority ");
                parameters.Add("priority", (int)filter.Priority.Value);
            }

            if (filter.Category.HasValue)
            {
                filterSql.Append(" AND Category = @category ");
                parameters.Add("category", (int)filter.Category.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterSql.Append(" AND (Title LIKE @searchText OR Description LIKE @searchText) ");
                parameters.Add("searchText", $"%{filter.SearchText}%");
            }

            var sql = new StringBuilder();

            sql.AppendLine("SELECT COUNT(*) FROM Requests" + filterSql + ";");

            sql.AppendLine("SELECT Id, Title, Description, Category, Priority, Status, CreatedAt FROM Requests");
            sql.Append(filterSql);
            sql.AppendLine(" ORDER BY CreatedAt DESC OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;");

            var page = filter.Page < 1 ? 1 : filter.Page;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            parameters.Add("skip", (page - 1) * pageSize);
            parameters.Add("take", pageSize);

            using var multi = await connection.QueryMultipleAsync(sql.ToString(), parameters);

            var totalCount = await multi.ReadFirstAsync<int>();
            var items = await multi.ReadAsync<RequestListingDto>();

            return new PagedResult<RequestListingDto>(items, totalCount);
        }

        public async Task<IEnumerable<RequestHistoryDto>> GetRequestHistoryAsync(Guid requestId, string userId, string role)
        {
            using var connection = dbConnection.CreateConnection();

            var sql = """
                        SELECT 
                            CAST(h.FromStatus AS VARCHAR) AS FromStatus,
                            CAST(h.ToStatus AS VARCHAR) AS ToStatus,
                            h.ChangedBy,
                            h.ChangedAt,
                            h.Comment
                        FROM RequestHistories h
                        INNER JOIN Requests r ON h.RequestId = r.Id
                        WHERE h.RequestId = @requestId
                          AND (@role = 'Manager' OR r.CreatedByUserId = @userId)
                        ORDER BY h.ChangedAt DESC
                    """;

            var history = await connection.QueryAsync<RequestHistoryDto>(sql, new { requestId, userId, role });

            return history;
        }
    }
}
