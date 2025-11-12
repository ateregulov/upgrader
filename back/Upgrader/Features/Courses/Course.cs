using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Upgrader.Features.Courses;

public class Course
{
    public Guid Id { get; set; }
    public List<Tasks.Task> Tasks { get; set; } = [];

    [Required]
    public string Title { get; set; }

    [Required]
    public string ShortDescription { get; set; }

    [Required]
    public string LongDescription { get; set; }
    public decimal Price { get; set; }

    [NotMapped]
    public bool IsBought { get; set; }
    public List<CoursePurchase> Purchases { get; set; } = [];
}
