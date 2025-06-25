using DapperPractice.Abstractions;

namespace DapperPractice.Entities;

public sealed class Student : Entity
{

    public string Name { get; init; } = null!;
    public int Age { get; init; }

    public ICollection<Course> Courses { get; private set; } = new List<Course>();
    
    public static Student Create(string studentName, string studentAge)
    {
        return new Student()
        {
            Id = Guid.NewGuid(),
            Name = studentName,
            Age = int.Parse(studentAge)
        };
    }

    public void UpdateCourse(Course course)
    {
        if (Courses.Contains(course))
        {
            Courses.Remove(course);
            Courses.Add(course);
        }
        Courses.Add(course);
    }

    public void AddCourse(Course course)
    {
        Courses.Add(course);
    }

    public void RemoveCourse(string courseName)
    {
        var course = Courses.FirstOrDefault(x => string.Equals(x.Name, courseName, StringComparison.OrdinalIgnoreCase));
        if (course is not null)
        {
            Courses.Remove(course);
        }
    }
    
}