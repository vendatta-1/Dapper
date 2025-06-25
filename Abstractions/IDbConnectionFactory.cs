namespace DapperPractice.Abstractions;

public interface IDbConnectionFactory
{
    string ConnectionString(DbContextType dbContextType);
}