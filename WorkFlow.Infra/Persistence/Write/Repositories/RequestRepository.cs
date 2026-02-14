using Microsoft.EntityFrameworkCore;
using WorkFlow.Domain.Interfaces;
using WorkFlow.Domain.Models;
using WorkFlow.Infra.Persistence.Write.Context;

namespace WorkFlow.Infra.Persistence.Write.Repositories
{
    internal class RequestRepository(AppDbContext context) : IRequestRepository
    {
        public async Task AddAsync(RequestModel request)
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync();
        }

        public async Task<RequestModel?> GetByIdAsync(Guid id)
        {
            return await context.Requests
                .Include(r => r.History)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Guid requestId,RequestModel request)
        {
            foreach (var history in request.History)
            {
                var entry = context.Entry(history);
                if (entry.State == EntityState.Detached || entry.Property("Id").CurrentValue.Equals(Guid.Empty))
                {
                    entry.State = EntityState.Added;
                }
            }
            context.Requests.Update(request);
            await context.SaveChangesAsync();
        }
    }
}
