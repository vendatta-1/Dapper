using System.ComponentModel.DataAnnotations;
using DapperPractice.Abstractions;

namespace DapperPractice.Entities;

public sealed class AuditLog : Entity
{
     
    [MaxLength(100)]
    public string TableName { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Operation { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string Sql { get; set; } = string.Empty;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public long DurationMs { get; set; }
}