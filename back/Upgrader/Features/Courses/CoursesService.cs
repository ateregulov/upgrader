using Microsoft.EntityFrameworkCore;

namespace Upgrader.Features.Courses;

public class CoursesService
{
    private readonly MyContext _dbContext;

    public CoursesService(MyContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Course>> GetAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        var courses = await _dbContext
            .Courses.Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                Price = x.Price,
                IsBought = x.Purchases.Any(x => x.UserId == userId),
                TasksCount = x.Tasks.Count,
                FinishedTasksCount = x.Tasks.Count(x => x.Results.Any(x => x.UserId == userId)),
            })
            .OrderByDescending(x => x.IsBought)
            .ToListAsync(cancellationToken);

        return courses;
    }

    public async Task<Course> GetByIdAsync(
        Guid userId,
        Guid courseId,
        CancellationToken cancellationToken = default
    )
    {
        var course = await _dbContext
            .Courses.Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                Price = x.Price,
                IsBought = x.Purchases.Any(x => x.UserId == userId),
                TasksCount = x.Tasks.Count,
                FinishedTasksCount = x.Tasks.Count(x => x.Results.Any(x => x.UserId == userId)),
            })
            .FirstOrDefaultAsync(x => x.Id == courseId, cancellationToken);

        return course;
    }
}
