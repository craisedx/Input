namespace Input.Constants.Admin
{
    public static class AdminConstants
    {
        public const string AdminRole = "Admin";
        public const string AdminAccountName = "Admin";
        public const string AdminAccountEmail = "admin@input.org";
        public const string AdminAccountAvailable = "Администратор уже имеется!";
        public const string AdminCreated = "Администратор успешно создан!";
        public const string AdminPassword = "123Snp-";
        public static string AdminRoleAvailable(string status) {
            return 
                $"Роль '{status}' уже имеется!";
        }
        public static string AdminRoleCreated(string status) {
            return 
                $"Роль '{status}' успешно создана!";
        }
    }
}