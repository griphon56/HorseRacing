namespace HorseRacing.Domain.Common.Models.Authentication
{
    public class HashPasswordSettings
    {
        /// <summary>
        /// Название раздела.
        /// </summary>
        public const string SectionName = "HashPasswordSettings";
        /// <summary>
        /// Идентификатор сведений о массиве байт для шифрования
        /// </summary>
        public string HashHexString { get; init; } = string.Empty;
    }
}
