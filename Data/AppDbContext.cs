using DapperPractice.Abstractions;
using DapperPractice.Configurations;
using DapperPractice.Entities;
using Microsoft.EntityFrameworkCore;

namespace DapperPractice.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : 
    DbContext(options), IUnitOfWork
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new StudentConfigurations());
    }
}