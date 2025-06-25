using Microsoft.EntityFrameworkCore;

namespace DapperPractice.Entities.CourseValueObjs;

[Owned]
public record Duration
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    private Duration() { }

    private Duration(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start date must be before end date");

        StartDate = start;
        EndDate = end;
    }

    public static Duration Create(DateTime start, DateTime end) => new(start, end);
}
