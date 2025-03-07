using Infrastructure.Models;

namespace ECourse.Services.AuthAPI.Models
{
    /// <summary>
    /// Here we define social networks which may be used by users and teachers
    /// such as youtube, facebook, instagram and ....
    /// </summary>
    public class SocialLink : BaseEntity
    {
        public static readonly string DocumentName = nameof(SocialLink);
        /// <summary>
        /// Name of social media
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Set the Origin file name plus domain name because to show icon we bind Icon to image url tag
        /// </summary>
        public required string Icon { get; set; }
        /// <summary>
        /// Set Origin File name
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// save relative file name for delete
        /// </summary>
        public string FileLocation { get; set; } = string.Empty;
    }
}
