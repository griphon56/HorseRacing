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
        /// <summary>
        /// Количество преград
        /// </summary>
        public const int NumberOfObstacles = 6;
        /// <summary>
        /// Количество игроков
        /// </summary>
        public const int NumberOfPlayers = 4;
        /// <summary>
        /// Количество лошадей
        /// </summary>
        public const int NumberOfHorse = 4;

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
