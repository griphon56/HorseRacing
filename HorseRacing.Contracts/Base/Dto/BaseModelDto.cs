namespace HorseRacing.Contracts.Base.Dto
{
    public class BaseModelDto : BaseDto
    {
        public Guid Id { get; set; }

        public BaseModelDto() { }
        public BaseModelDto(Guid id)
        {
            Id = id;
        }
    }
}
