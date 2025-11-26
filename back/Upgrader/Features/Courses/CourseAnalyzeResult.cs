using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Upgrader.Features.Courses;

public class CourseAnalyzeResult
{
    [Key]
    [ForeignKey(nameof(Request))]
    public Guid RequestId { get; set; }
    public CourseAnalyzeRequest Request { get; set; }
    public string Message { get; set; }
}
