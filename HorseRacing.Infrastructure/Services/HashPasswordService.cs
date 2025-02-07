using HorseRacing.Application.Common.Interfaces.Services;
using HorseRacing.Domain.Common.Models.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace HorseRacing.Infrastructure.Services
{
    /// <summary>
    /// Сервис хэширования паролей
    /// </summary>
    public class HashPasswordService : IHashPasswordService
    {
        const int keySize = 58;
        const int iterations = 341000;
        private readonly byte[] _salt;
        private readonly byte[] _encryptionKey;

        /// <summary>
        /// Генератор токенов jwt.
        /// </summary>
        private readonly HashPasswordSettings _hashOptions;
        public HashPasswordService(IOptions<HashPasswordSettings> hashOptions)
        {
            _hashOptions = hashOptions.Value;
            if (string.IsNullOrEmpty(_hashOptions.HashHexString))
            {
                throw new Exception("Не задан параметр для шифрования паролей!");
            }
            _salt = Convert.FromHexString(_hashOptions.HashHexString);
            _encryptionKey = new Rfc2898DeriveBytes(_hashOptions.HashHexString, _salt, iterations, HashAlgorithmName.SHA512).GetBytes(32); // 256-bit key
        }
        /// <summary>
        /// Метод генерации хэш для пароля
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        public string HashPassword(string password)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                _salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);

        }
        /// <summary>
        /// Метод проверки пароля (верификация)
        /// </summary>
        /// <param name="password">Исходный пароль</param>
        /// <param name="hashedPassword">Хэш пароля</param>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, _salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hashedPassword));
        }
        /// <summary>
        /// Метод шифрования текста
        /// </summary>
        /// <param name="plainText">Исходный текст</param>
        public byte[] Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Метод дешифрования текста
        /// </summary>
        /// <param name="cipherText">Зашифрованный текст</param>
        /// <returns>Декодированный текст</returns>
        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromHexString(cipherText);
            return DecryptBytes(fullCipher);
        }

        /// <summary>
        /// Метод дешифрования текста из байтового массива
        /// </summary>
        /// <param name="cipherBytes">Зашифрованный текст в виде массива байтов</param>
        /// <returns>Декодированный текст</returns>
        public string DecryptBytes(byte[] cipherBytes)
        {
            if (_encryptionKey == null || _encryptionKey.Length == 0)
            {
                return string.Empty;
            }
            if (cipherBytes == null || cipherBytes.Length == 0)
            {
                return string.Empty;
            }
            using (var aes = Aes.Create())
            {
                aes.Key = _encryptionKey;
                var iv = new byte[aes.BlockSize / 8];
                Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                {
                    using (var ms = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
