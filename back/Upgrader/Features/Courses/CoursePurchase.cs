using Upgrader.Users;

namespace Upgrader.Features.Courses;

public class CoursePurchase
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid CourseId { get; set; }
    public Course Course { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTimeOffset PurchasedDate { get; set; } = DateTimeOffset.UtcNow;
}
