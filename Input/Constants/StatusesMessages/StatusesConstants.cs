namespace Input.Constants.Statuses
{
    public static class StatusesConstants
    {
        public const string AwaitProcessingStatus = "Ожидает обработки";
        public const string ProcessedStatus = "Принят в обработку";
        public const string RejectedStatus = "Отклонен";
        public const string ApprovedStatus = "Подтвержден";
        public const string BlockedStatus = "Заблокирован";
        public static string StatusAvailable(string status) {
            return 
                $"Статус '{status}' уже имеется!";
        }
        public static string StatusCreated(string status) {
            return 
                $"Статус '{status}' успешно создан!";
        }
    }
}