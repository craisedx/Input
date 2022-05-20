namespace Input.Constants.InfoMessages
{
    public static class UserInfoConstants
    {
        public const string Password = "Password";
        public const string BaseUserPhoto = "https://img.icons8.com/ios-glyphs/200/000000/user-menu-male.png";
        public const string EmailName = "Input - Сервис с авторскими сочинениями";
        public const string EmailService = "fanfanconfim@fanfan.com";
        public const string EmailServiceGmail = "fanfanconfim@gmail.com";
        public const string PasswordService = "stbnkqraincqwfet";
        public const string ConnectHost = "smtp.gmail.com";
        public const string AccountConfirmation = "Подтверждение аккаунта";
        
        public static string SendConfirmEmail(string callbackUrl) {
            return 
                $"Подтвердите регистрацию, перейдя по ссылке: <a style=\"font-size:48px;\" href=\"{callbackUrl}\">link</a>";
        }
    }
}