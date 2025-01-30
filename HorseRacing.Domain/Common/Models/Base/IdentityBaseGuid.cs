namespace HorseRacing.Domain.Common.Models.Base
{
    public class IdentityBaseGuid : ValueObject
    {
        /// <summary>
        /// Идентификатор сведений о значении.
        /// </summary>
        public Guid Value { get; private set; }

        protected IdentityBaseGuid() : base() { }

        protected IdentityBaseGuid(Guid id)
        {
            Value = id;
        }

        /// <summary>
        /// Метод создания идентификатора
        /// </summary>
        /// <param name="id">идентификатор.</param>
        /// <returns>Возвращает идентификатор <see cref="IdentityBaseGuid"/></returns>
        public static IdentityBaseGuid CreateId(Guid id)
        {
            return new(id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;

        }
    }
}
