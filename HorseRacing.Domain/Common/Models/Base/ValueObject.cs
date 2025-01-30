using HorseRacing.Domain.Common.Interfases;

namespace HorseRacing.Domain.Common.Models.Base
{
    public abstract class ValueObject : IEquatable<ValueObject>, IValueObject
    {
        /// <summary>
        /// Получает компоненты равенства.
        /// </summary>
        public abstract IEnumerable<object> GetEqualityComponents();

        protected ValueObject() { }
        /// <summary>
        /// Сравнения объектов на равенство компонентов ValueObject и GetEqualityComponents().
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType()) return false;
            var valueObj = obj as ValueObject;
            return GetEqualityComponents().
                SequenceEqual(valueObj!.GetEqualityComponents());
        }

        public static bool operator ==(ValueObject left, ValueObject right) { return Equals(left, right); }
        public static bool operator !=(ValueObject left, ValueObject right) { return !Equals(left, right); }
        /// <summary>
        /// Генерирует хеш-код объекта на основе хеш-кодов его компонентов, используя операцию XOR.
        /// </summary>
        public override int GetHashCode()
        {
            return GetEqualityComponents().Select(m => m?.GetHashCode() ?? 0).
                Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Метод Equals определён для сравнения объектов типа ValueObject.
        /// </summary>
        public bool Equals(ValueObject? other)
        {
            return Equals((object?)other);
        }
    }
}
