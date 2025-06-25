using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DapperPractice.Data;


public class AuditDbContextFactory : IDesignTimeDbContextFactory<AuditDbContext>
{
    public AuditDbContext CreateDbContext(string[] args)
    {
        var options = DesignTimeFactoryHelper.BuildOptions<AuditDbContext>("auditDb",
            (builder, conn) => builder.UseSqlite(conn));

        return new AuditDbContext(options);
    }
}
