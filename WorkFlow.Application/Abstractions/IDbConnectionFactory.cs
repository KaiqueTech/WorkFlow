using System.Data;

namespace WorkFlow.Application.Abstractions
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
