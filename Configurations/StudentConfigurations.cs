using DapperPractice.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DapperPractice.Configurations;

public sealed class StudentConfigurations : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasMaxLength(250)
            .IsRequired();
        builder.HasMany(x => x.Courses)
            .WithMany(c => c.Students);
    }
}