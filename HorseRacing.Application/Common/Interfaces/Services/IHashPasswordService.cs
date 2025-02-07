namespace HorseRacing.Application.Common.Interfaces.Services
{
    public interface IHashPasswordService
    {
        /// <summary>
		/// Генерируем хэш для пароля
		/// </summary>
		/// <param name="password">Исходный пароль</param>
		string HashPassword(string password);
        /// <summary>
        /// Метод проверки пароля (верификация)
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <param name="hashedPassword">Хэш пароля</param>
        bool VerifyPassword(string password, string hashedPassword);
        /// <summary>
        /// Метод шифрования текста
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        /// <returns>Зашифрованный текст</returns>
        byte[] Encrypt(string plainText);

        /// <summary>
        /// Метод дешифрования текста
        /// </summary>
        /// <param name="cipherText">Зашифрованный текст</param>
        /// <returns>Декодированный текст</returns>
        string Decrypt(string cipherText);

        /// <summary>
        /// Метод дешифрования текста из байтового массива
        /// </summary>
        /// <param name="cipherBytes">Зашифрованный текст в виде массива байтов</param>
        /// <returns>Декодированный текст</returns>
        string DecryptBytes(byte[] cipherBytes);
    }
}
