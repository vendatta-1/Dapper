using System.Diagnostics;
using System.Linq.Expressions;
using Dapper;
using DapperPractice.Abstractions;
using DapperPractice.Data;
using DapperPractice.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DapperPractice.Logic;

public class BaseRepository<T> : IBaseRepository<T>
    where T : Entity, new()
{
    private readonly string ConnectionString;
    private readonly DbContext Context;
    private readonly DbSet<T> _dbSet;
    private readonly IDbContextFactory<AuditDbContext> _auditFactory;

    public BaseRepository(
        IDbContextFactory<AppDbContext> factory,
        IDbContextFactory<AuditDbContext> auditFactory,
        IDbConnectionFactory connectionFactory)
    {
        Context = factory.CreateDbContext();
        _auditFactory = auditFactory;
        ConnectionString = connectionFactory.ConnectionString(DbContextType.AppDbContext);
        _dbSet = Context.Set<T>();
    }

    private async Task LogOperationAsync(string tableName, string operation, string sql, long elapsedMs)
    {
        await using var auditContext = _auditFactory.CreateDbContext();
        auditContext.AuditLogs.Add(new AuditLog
        {
            TableName = tableName,
            Operation = operation,
            Sql = sql,
            Timestamp = DateTime.UtcNow,
            DurationMs = elapsedMs
        });
        await auditContext.SaveChangesAsync();
    }

    public async Task<int> Add(T entity)
    {
        var sw = Stopwatch.StartNew();
        await _dbSet.AddAsync(entity);
        var result = await Context.SaveChangesAsync();
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "INSERT", $"EF INSERT INTO {typeof(T).Name}s", sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<int> Update(T entity)
    {
        var sw = Stopwatch.StartNew();
        var isExist = await Exists(T => T.Id == entity.Id);
        if (!isExist) throw new Exception("Entity not found");

        _dbSet.Update(entity);
        var result = await Context.SaveChangesAsync();
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "UPDATE", $"EF UPDATE {typeof(T).Name}s", sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<int> Delete(T entity)
    {
        var sql = $"DELETE FROM {typeof(T).Name}s WHERE Id = @Id";
        var sw = Stopwatch.StartNew();
        using var connection = new SqlConnection(ConnectionString);
        var result = await connection.ExecuteAsync(sql, new { entity.Id });
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "DELETE", sql, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var sql = $"SELECT * FROM {typeof(T).Name}s";
        var sw = Stopwatch.StartNew();
        using var connection = new SqlConnection(ConnectionString);
        var result = await connection.QueryAsync<T>(sql);
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "SELECT", sql, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<IEnumerable<T>> GetBy(Expression<Func<T, bool>> predicate)
    {
        var sw = Stopwatch.StartNew();
        var query = _dbSet.Where(predicate);
        var sql = query.ToQueryString();
        var result = await query.ToListAsync();
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "SELECT", sql, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<IEnumerable<T>> GetBy(Expression<Func<T, bool>> predicate, int skip, int take)
    {
        var sw = Stopwatch.StartNew();
        var query = _dbSet.Where(predicate).Skip(skip).Take(take);
        var sql = query.ToQueryString();
        var result = await query.ToListAsync();
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "SELECT-PAGINATED", sql, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<int> GetCount()
    {
        var sql = $"SELECT COUNT(*) FROM {typeof(T).Name}s";
        var sw = Stopwatch.StartNew();
        using var connection = new SqlConnection(ConnectionString);
        var result = await connection.ExecuteScalarAsync<int>(sql);
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "COUNT", sql, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<int> GetCount(Expression<Func<T, bool>> predicate)
    {
        var sw = Stopwatch.StartNew();
        var result = await _dbSet.CountAsync(predicate);
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "COUNT-PREDICATE", _dbSet.Where(predicate).ToQueryString(), sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<T?> Find(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<T?> Find(Guid id)
    {
        var sql = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
        var sw = Stopwatch.StartNew();
        using var connection = new SqlConnection(ConnectionString);
        var result = await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        sw.Stop();
        await LogOperationAsync(typeof(T).Name, "FIND", sql, sw.ElapsedMilliseconds);
        return result;
    }
}
