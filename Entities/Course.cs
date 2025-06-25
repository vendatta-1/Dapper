using DapperPractice.Abstractions;
using DapperPractice.Entities.CourseValueObjs;
using Microsoft.EntityFrameworkCore;

namespace DapperPractice.Entities;

public sealed  class Course : Entity
{
    public string Name { get; init; } = null!;
    
    public Duration CourseDuration { get; init; }
    public ICollection<Student> Students { get; init; } = new List<Student>();
}