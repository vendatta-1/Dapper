using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DapperPractice.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = DesignTimeFactoryHelper.BuildOptions<AppDbContext>("appDb",
            (builder, conn) => builder.UseNpgsql(conn));

        return new AppDbContext(options);
    }
}