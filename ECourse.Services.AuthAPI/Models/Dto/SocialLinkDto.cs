using Infrastructure.Models;

namespace ECourse.Services.AuthAPI.Models.Dto
{
    public class SocialLinkDto:BaseDto
    {
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
