using DapperPractice.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DapperPractice.Logic;

public sealed class DbConnectionFactory : IDbConnectionFactory
{
   
    private readonly IConfiguration _config;

    public DbConnectionFactory(IConfiguration config)
    {
        _config = config;
    }

    public string ConnectionString(DbContextType type) => type switch
    {
        DbContextType.AppDbContext => _config.GetConnectionString("appDb")??throw new InvalidOperationException("appDb not set"),
        DbContextType.AuditDbContext => _config.GetConnectionString("auditDb")??throw new InvalidOperationException("auditDb not set"),
        DbContextType.AuthDbContext => _config.GetConnectionString("authDb")??throw new InvalidOperationException("authDb not set"),
        _ => throw new NotSupportedException()
    };
}