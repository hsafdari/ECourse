
namespace ECourse.Admin.Models;

public class BaseDto
{
    public string Id { get; set; }=Guid.NewGuid().ToString();
    public DateTime CreateDateTime { get; set; } = DateTime.Now;
    public DateTime? ModifiedDateTime { get; set; }
    public bool IsDeleted { get; set; } = false;
}
