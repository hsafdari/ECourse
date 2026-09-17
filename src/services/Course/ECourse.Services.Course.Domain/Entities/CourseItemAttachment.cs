

using ECourse.Services.Course.Domain.Common;

namespace ECourse.Services.Course.Domain.Entities;
public class CourseItemAttachment : AggregateRoot<Guid>
{
    public int CourseItemId { get; set; }
    public string FileName { get; set; } = string.Empty;
    //Relative file location
    public string FileLocation { get; set; } = string.Empty;
    //Link of file which can be downloaded
    public string FileUrl { get; set; }= string.Empty;
    /// <summary>
    /// file type extensions,auto filled
    /// </summary>
    public string FileExtension { get; set; } = string.Empty;
}

