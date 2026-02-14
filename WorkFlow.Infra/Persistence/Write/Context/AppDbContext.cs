using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkFlow.Domain.Models;

namespace WorkFlow.Infra.Persistence.Write.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUserModel>(options)
    {

        public DbSet<RequestModel> Requests { get; set; }
        public DbSet<RequestHistoryModel> RequestHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
