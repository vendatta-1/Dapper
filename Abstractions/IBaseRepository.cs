using System.Linq.Expressions;
using DapperPractice.Entities;

namespace DapperPractice.Abstractions;

public interface IBaseRepository<T>
    where T:Entity,new()
{
    Task<int> Add(T entity);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetBy(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetBy(Expression<Func<T, bool>> predicate, int skip, int take);
    Task<int> GetCount();
    Task<int> GetCount(Expression<Func<T, bool>> predicate);
    Task<bool> Exists(Expression<Func<T, bool>> predicate);
    Task<T?> Find(Expression<Func<T, bool>> predicate);
    Task<T?> Find(Guid id);
}