namespace Mini_HR_app.Data.Models.Constants.Utils
{
    public static class UserRole
    {
        public const string ADMIN = "Administrator";
        public const string MANAGER = "Manager";
        public const string MEMBER = "Member";
        public const string ADMIN_MANAGER = ADMIN + "," + MANAGER;
        public const string ADMIN_MANAGER_MEMBER = ADMIN + "," + MANAGER + "," + MEMBER;
    }
}
