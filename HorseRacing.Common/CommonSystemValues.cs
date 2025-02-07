namespace HorseRacing.Common
{
    /// <summary>
    /// Класс общих системных значений.
    /// </summary>
    public static class CommonSystemValues
    {
        /// <summary>
        /// Название системы по умолчанию.
        /// </summary>
        public const string DefaultSystemUserName = "Система";

        public static class CommonRepositoryValues
        {
            /// <summary>
            /// Максимальное количество записей для получения из запроса
            /// </summary>
            public const int MaxTakeInContainsQuery = 500;
            /// <summary>
            /// Пороговое число записей, при котором необходимо выполнять выборку по частям.
            /// </summary>
            public const int ContainsThresholdValue = 1000;
        }
    }
}
