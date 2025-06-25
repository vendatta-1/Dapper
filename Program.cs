using DapperPractice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dapper;
using DapperPractice.Abstractions;
using DapperPractice.Entities;
using DapperPractice.Entities.CourseValueObjs;
using DapperPractice.Logic;
using DapperPractice.Service;
using Npgsql;
using SQLitePCL;

Batteries.Init();
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration;
        //
        // services.AddDbContext<AppDbContext>(options =>
        //     options.UseNpgsql(config.GetConnectionString("appDb"))
        //         .UseSnakeCaseNamingConvention());
        services.AddDbContext<AuditDbContext>(opt =>
        {
            opt.UseSqlite("Data Source=audit.db");
        });
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

        services.AddDbContextFactory<AppDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("appDb"));
        });
        services.AddDbContextFactory<AuditDbContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("auditDb"));
        });
        services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
        
        services.AddScoped<IStudentService, StudentService>();
    })
    .Build();
    
    
using var scope = host.Services.CreateScope();
var audit= scope.ServiceProvider.GetService<AuditDbContext>();
Console.WriteLine(audit.Database.EnsureCreated());

Console.WriteLine(audit.Database.GetDbConnection().ToString());
var studentService = scope.ServiceProvider.GetRequiredService<IStudentService>();
var courses = new List<Course>()
{
    new Course()
    {
        Name = "CS101",
        CourseDuration = Duration.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(90)),
    },
    new Course()
    {
        Name = "CS102",
        CourseDuration = Duration.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(90)),
        
    }
};
// var student = new Student()
// {
//     Age = 19,
//     Name = "Abdullah khaled",
//     
// };
// student.AddCourse(courses[0]);
// student.AddCourse(courses[1]);
//
//
// await studentService.CreateAsync(student);

var student = await studentService.GetStudent(Guid.Parse("fb92d5ef-39ee-4c9a-951b-59d9326d148f"));
Console.WriteLine(student.Name);