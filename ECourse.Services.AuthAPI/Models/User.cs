using ECourse.Services.AuthAPI.Enums;
using Microsoft.AspNetCore.Identity;

namespace ECourse.Services.AuthAPI.Models
{
    public class User : IdentityUser<string>
    {
        #region Properties       
        public bool IsSystemAccount { get; set; }
        public bool IsBanned { get; set; }
        public StatusUser Status { get; set; }

        /// <summary>
        /// the code which requested by user
        /// </summary>
        public string? RecoveryPasswordCode { get; set; }
        public DateTime? RecoveryPasswordCreatedDateTime { get; set; }
        public DateTime? RecoveryPasswordExpireDate { get; set; }
        public bool RecoveryPasswordStatus { get; set; } = false;
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int RowVersion { get; set; }
        #endregion

        #region Collection navigation properties
        public List<UserRole> UserRoles { get; set; } = null!;
        #endregion
    }
}
