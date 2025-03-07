using Microsoft.AspNetCore.Identity;

namespace ECourse.Services.AuthAPI.Models
{
    public class Role : IdentityRole<string>
    {
        public bool IsSystemRole { get; set; }
        /// <summary>
        /// Is This role category banned?
        /// </summary>
        public bool IsBanned { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int RowVersion { get; set; }
        #region Collection navigation properties       
        public List<UserRole> UserRoles { get; set; } = null!;
        #endregion
    }
}
