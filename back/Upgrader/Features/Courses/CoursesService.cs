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
        bool isLocal = true,
        CancellationToken cancellationToken = default
    )
    {
        var boughtCoursesIds = await _dbContext
            .CoursePurchases
            .Where(x => isLocal ? x.UserId == userId : x.ExternalUserId == userId)
            .Select(x => x.CourseId)
            .ToHashSetAsync(cancellationToken);

        var finishedTasksCounts = await _dbContext.TaskResults
            .Where(x => isLocal ? x.UserId == userId : x.ExternalUserId == userId)
            .GroupBy(x => x.Task.CourseId)
            .ToDictionaryAsync(x => x.Key, x => x.Count(), cancellationToken);

        var courses = await _dbContext
            .Courses.Select(x => new Course
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                Price = x.Price,
                IsBought = boughtCoursesIds.Contains(x.Id),
                TasksCount = x.Tasks.Count,
                FinishedTasksCount = finishedTasksCounts.GetValueOrDefault(x.Id, 0),
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
