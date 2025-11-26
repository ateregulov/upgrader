using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Upgrader.Features.Courses;

public class CourseAnalyzeResult
{
    [Key]
    [ForeignKey(nameof(Request))]
    public Guid RequestId { get; set; }

    [JsonIgnore]
    public CourseAnalyzeRequest Request { get; set; }
    public string Message { get; set; }
}
