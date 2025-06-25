using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DapperPractice.Data;

public static class DesignTimeFactoryHelper
{
    public static DbContextOptions<T> BuildOptions<T>(string connectionName, Action<DbContextOptionsBuilder<T>, string> configure)
        where T : DbContext
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connStr = config.GetConnectionString(connectionName);
        var optionsBuilder = new DbContextOptionsBuilder<T>();
        configure(optionsBuilder, connStr);

        return optionsBuilder.Options;
    }
}