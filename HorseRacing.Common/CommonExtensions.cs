using System.ComponentModel;
using System.Reflection;

namespace HorseRacing.Common
{
    public static class CommonExtensions
    {
        /// <summary>
		/// Получаем информацию об описании перечисления
		/// </summary>
		/// <param name="value">Перечисление</param>
		/// <param name="LowerCase">В нижнем регистре</param>
		public static string GetDescription(this Enum value, bool LowerCase = false)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString())!;
            DescriptionAttribute[] attributes = 
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return LowerCase ? (attributes[0].Description ?? "").ToLower() : attributes[0].Description;
            }
            else
            {
                return LowerCase ? value.ToString().ToLower() : value.ToString();
            }
        }
    }
}
