using DapperPractice.Entities;
using Microsoft.EntityFrameworkCore;

namespace DapperPractice.Data;

public sealed class AuditDbContext(DbContextOptions options):DbContext(options)
{
    
    public DbSet<AuditLog> AuditLogs { get; set; }
}