using Input.ViewModels.FanFiction;

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
        public const string InfoMessage = "Информация";
        public const string NewPublication = "Подтверждение аккаунта";
        
        public static string SendConfirmEmail(string username,string callbackUrl) {
            return 
                $"<h2 style=\"color: black\">Здравствуйте, {username}!</h2> <p style=\"font-size: 16px; color: black\">Для подтверждения регистрации аккаунта на проекте <strong style=\"font-size: 18px\">Input</strong>, нажмите на кнопку ниже.</p>" +
                "<p style=\"color: gray; font-size: 16px;\">Если это не Вы, пожалуйста, проигнорируйте данное сообщение</p> <br/>" +
                $"<a style=\"  display: inline-block;background: #238B49;color: #fff;padding: 12px;border-radius: 3px;text-decoration: none;font-family: Tahoma;font-size: 18px;line-height: 1;font-weight: 100;\" href =\"{callbackUrl}\">Подтвердить</a>";
        }
        
        public static string SendAdminToNewPublicationEmail(string username, string callbackUrl) {
            return 
                $"<h2 style=\"color: black\">Здравствуйте, {username}!</h2> <p style=\"font-size: 16px; color: black\">Пришло новое сочинение ожидает публикацию, нажмите на кнопку ниже чтобы перейти.</p>" +
                "<p style=\"color: gray; font-size: 16px;\">Если это не Вы, пожалуйста, проигнорируйте данное сообщение</p> <br/>" +
                $"<a style=\"  display: inline-block;background: #238B49;color: #fff;padding: 12px;border-radius: 3px;text-decoration: none;font-family: Tahoma;font-size: 18px;line-height: 1;font-weight: 100;\" href =\"{callbackUrl}\">Перейти</a>";
        }
        
        public static string SendUserToAddAdminToPublicationEmail(FanFictionViewModel fanFiction, string callbackUrl) {
            return 
                $"<h2 style=\"color: black\">Здравствуйте, {fanFiction.User.UserName}!</h2> <p style=\"font-size: 16px; color: black\">Ваше сочинение <strong>{fanFiction.Name}</strong> взял на проверку администратор - <strong>{fanFiction.Moderation.User.UserName}</strong>.</p>" +
                "<p style=\"color: gray; font-size: 16px;\">Если это не Вы, пожалуйста, проигнорируйте данное сообщение</p> <br/>" +
                $"<a style=\"  display: inline-block;background: #238B49;color: #fff;padding: 12px;border-radius: 3px;text-decoration: none;font-family: Tahoma;font-size: 18px;line-height: 1;font-weight: 100;\" href =\"{callbackUrl}\">Перейти</a>";
        }
        
        public static string SendUserToUpdatePublicationEmail(FanFictionViewModel fanFiction, string callbackUrl) {
            return 
                $"<h2 style=\"color: black\">Здравствуйте, {fanFiction.User.UserName}!</h2> <p style=\"font-size: 16px; color: black\">На вашем сочинении <strong>{fanFiction.Name}</strong> администратор - <strong>{fanFiction.Moderation.User.UserName}</strong> изменил статус, нажмите на сообщение ниже, чтобы перейти.</p>" +
                "<p style=\"color: gray; font-size: 16px;\">Если это не Вы, пожалуйста, проигнорируйте данное сообщение</p> <br/>" +
                $"<a style=\"  display: inline-block;background: #238B49;color: #fff;padding: 12px;border-radius: 3px;text-decoration: none;font-family: Tahoma;font-size: 18px;line-height: 1;font-weight: 100;\" href =\"{callbackUrl}\">Перейти</a>";
        }
        
        public static string SendUserApprovePublicationEmail(FanFictionViewModel fanFiction, string callbackUrl) {
            return 
                $"<h2 style=\"color: black\">Здравствуйте, {fanFiction.User.UserName}!</h2> <p style=\"font-size: 16px; color: black\">Ваше сочинение <strong>{fanFiction.Name} <span style=\"color:green\">успешно</span></strong> прошло модерацию и было опубликовано! Нажмите на сообщение ниже, чтобы перейти.</p>" +
                "<p style=\"color: gray; font-size: 16px;\">Если это не Вы, пожалуйста, проигнорируйте данное сообщение</p> <br/>" +
                $"<a style=\"  display: inline-block;background: #238B49;color: #fff;padding: 12px;border-radius: 3px;text-decoration: none;font-family: Tahoma;font-size: 18px;line-height: 1;font-weight: 100;\" href =\"{callbackUrl}\">Перейти</a>";
        }
    }
}