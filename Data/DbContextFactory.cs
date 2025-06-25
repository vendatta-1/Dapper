using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DapperPractice.Data;

public sealed class DbContextFactory<T> : IDbContextFactory<T>
    where T : DbContext
{
    public T CreateDbContext()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var type = typeof(T);
        var optionsBuilder = new DbContextOptionsBuilder<T>();

        if (type == typeof(AppDbContext))
        {
            var connStr = config.GetConnectionString("appDb");
            optionsBuilder.UseNpgsql(connStr);
        }
        else if (type == typeof(AuditDbContext))
        {
            var connStr = config.GetConnectionString("auditDb");
            optionsBuilder.UseSqlite(connStr);
        }
        // else if (type == typeof(AuthDbContext))
        // {
        //     var connStr = config.GetConnectionString("authDb");
        //     optionsBuilder.UseSqlServer(connStr);
        // }
        else
        {
            throw new NotSupportedException($"Unsupported DbContext type: {type.Name}");
        }

        return (T)Activator.CreateInstance(typeof(T), optionsBuilder.Options)!;
    }
}