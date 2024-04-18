namespace ECourse.Admin.Utility
{
    public class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public const string Status_Pending = "Pending";
        public const string Status_Approved = "Approved";
        public const string Status_ReadyForPickup = "ReadyForPickup";
        public const string Status_Completed = "Completed";
        public const string Status_Refunded = "Refunded";
        public const string Status_Cancelled = "Cancelled";

        public static string CourseAPIBase { get; internal set; }
        public static string TokenCookie { get; internal set; }

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
