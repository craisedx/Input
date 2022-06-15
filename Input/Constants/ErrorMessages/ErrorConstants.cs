namespace Input.Constants
{
    public static class ErrorConstants
    {
        public const string ShortDescriptionRequired = "Короткое описание должно быть указано";
        public const string ShortDescriptionMaxLength = "Максимальная длина описания 1000 символов";
        public const string CommentBodyMaxLength = "Максимальная длина описания 500 символов";
        public const string CommentBodyRequired = "Комментарий пуст";
        public const string NameRequired = "Название должно быть указано";
        public const string NameMaxLength = "Максимальная длина имени 400 символов";
        public const string NameChapterMaxLength = "Максимальная длина названия 200 символов";
        public const string ChapterBodyMaxLength = "Максимальная длина содержания 5000 символов";
        public const string FandomRequired = "Тема должна быть указана";
        public const string ChapterBodyRequired = "Содержимое главы не может быть пустым";
        public const string ErrorAddAdminToFanFictionModeration = "Не удалось взять сочинение на проверку";
        public const string ErrorPublicationFanFiction = "Не удалось отправить сочинение на публикацию";
        public const string ErrorRemoveChapter = "Не удалось удалить главу";
        public const string ErrorRemoveFanFiction = "Не удалось удалить произведение";
        public const string ErrorRemoveComment = "Не удалось удалить комментарий";
        public const string PhotoRequired = "Фото должно быть загружено";
    }
}