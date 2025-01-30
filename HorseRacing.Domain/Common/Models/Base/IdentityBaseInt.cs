namespace HorseRacing.Domain.Common.Models.Base
{
    public class IdentityBaseInt : ValueObject
    {
        /// <summary>
        /// Значение
        /// </summary>
        public int Value { get; private set; }

        protected IdentityBaseInt() : base() { }

        protected IdentityBaseInt(int id)
        {
            Value = id;
        }

        /// <summary>
        /// Создает идентификатор <see cref="CreateId"/>
        /// </summary>
        public static IdentityBaseInt CreateId(int id)
        {
            return new(id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;

        }
    }
}
