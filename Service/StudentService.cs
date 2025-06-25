using DapperPractice.Abstractions;
using DapperPractice.Entities;

namespace DapperPractice.Service;

public sealed class StudentService(IBaseRepository<Student> repository):IStudentService
{

    public async Task<int> CreateAsync(Student student)
    {
        var res = await repository.Add(student);
        return res;
    }
    public async  Task<Student?> GetStudent(Guid uuid)
    {
       var student = await repository.Find(x=>x.Id==uuid);
       return student;
    }
    
}