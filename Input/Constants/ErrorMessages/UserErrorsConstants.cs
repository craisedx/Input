namespace Input.Constants
{
    public static class UserErrorsConstants
    {
        public const string NameNotProvided = "Не указано имя";
        public const string AddressNotSpecified = "Не указан адрес";
        public const string PasswordNotSpecified = "Не указан пароль";
        public const string PasswordEnteredIncorrectly = "Пароль введен неверно";
        public const string InvalidEmail = "Некорректный адрес";
        public const string EmailNotConfirmed = "Email не подтвержден";
        public const string LoginPasswordEnteredIncorrectly = "Логин или пароль введены неправильно";
        public const string ErrorMessageMinLength = "Длина имени должна быть от 3 до 50 символов";
        
        public static string UserNameAlreadyUse(string userName) {
            return $"Имя пользователя {userName} уже занято, попробуйте другое.";
        }
        public static string EmailAlreadyUse(string email) {
            return $"Почта {email} уже зарегистрирована, попробуйте другую.";
        }
    }
}