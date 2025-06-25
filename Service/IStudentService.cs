using DapperPractice.Entities;

namespace DapperPractice.Service;

public interface IStudentService
{
    Task<int> CreateAsync(Student student);
    Task<Student?> GetStudent(Guid uuid);
}