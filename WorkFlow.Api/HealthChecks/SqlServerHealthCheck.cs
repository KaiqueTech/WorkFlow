using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WorkFlow.Api.HealthChecks
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public SqlServerHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                await connection.OpenAsync(cancellationToken);

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    return HealthCheckResult.Healthy("Banco de dados SQL Server está acessível.");
                }

                return HealthCheckResult.Unhealthy("Não foi possível abrir conexão com o banco.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    "Erro ao conectar no SQL Server.",
                    ex);
            }
        }
    }
}
