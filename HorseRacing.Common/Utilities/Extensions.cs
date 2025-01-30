using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace HorseRacing.Common.Utilities
{
    public enum SeverityType
    {
        [Description("Критическая ошибка")]
        critical,
        [Description("Предупреждение")]
        warning,
        [Description("Информация")]
        info,
        [Description("Успешно")]
        success
    }

    public class EnumVisibility : Attribute
    {
        public EnumVisibility(bool Value)
        {
            this.Value = Value;
        }
        public bool Value { get; set; }
        public static bool GetEnumVisibility(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            EnumVisibility[] attributes = (EnumVisibility[])fi.GetCustomAttributes(typeof(EnumVisibility), false);
            bool DefaultVisibility = true;
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return DefaultVisibility;
            }
        }
    }

    public class EnumSeverity : Attribute
    {
        public EnumSeverity(SeverityType Value)
        {
            this.Value = Value;
        }
        public SeverityType Value { get; set; }
        public static SeverityType GetEnumSeverity(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            EnumSeverity[] attributes = (EnumSeverity[])fi.GetCustomAttributes(typeof(EnumSeverity), false);
            SeverityType DefaultSeverity = SeverityType.info;
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return DefaultSeverity;
            }
        }
    }

    public static class BaseTypesExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            return GetEnumDescriptionBase(value);
        }

        private static string GetEnumDescriptionBase(object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return "";
            }
            DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetClassDescription(this object value)
        {
            return value.GetType().GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault() ?? "";
        }

        public static string GetEnumCategory(this Enum value)
        {
            return GetEnumCategoryBase(value);
        }

        private static string GetEnumCategoryBase(object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            CategoryAttribute[] attributes = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Category;
            else
                return value.ToString();
        }
    }

    public static class ExceptionTo
    {
        public static string FormatExceptionToString(this Exception exc, bool AddStackTrace = true)
        {
            if (exc != null)
            {
                var StackTrace = FormatExceptionStackTraceToString(exc);
                string res = "";
                if (AddStackTrace)
                {
                    res = exc.Message + (exc.InnerException != null
                        ? " (" + FormatExceptionToString(exc.InnerException) + ")"
                        : "") + (!string.IsNullOrEmpty(StackTrace)
                            ? ". StackTrace:" + StackTrace
                            : "");
                }
                else
                {
                    res = exc.Message + (exc.InnerException != null
                        ? " (" + FormatExceptionToString(exc.InnerException) + ")"
                        : "");
                }
                return res;
            }
            return "";
        }

        public static string FormatExceptionStackTraceToString(this Exception exc)
        {
            if (exc.StackTrace == null)
            {
                return "";
            }
            string[] tempTrace = exc.StackTrace.Split('\n');
            string stackTrace = string.Empty;
            foreach (string trace in tempTrace)
            {
                stackTrace += trace;
            }

            return stackTrace;
        }
    }

    public static class Calculate
    {
        /// <summary>
        /// Метод получения MD5 хэша
        /// </summary>
        /// <param name="input">Массив байт</param>
        public static string GetMd5Hash(byte[] input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(input);

            // Create a new StringBuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
