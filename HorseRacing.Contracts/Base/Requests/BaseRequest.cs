using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Base.Requests
{
    /// <summary>
    /// Базовая модель запроса
    /// </summary>
    public class BaseRequest<T>
        where T : BaseDto
    {
        public T Data { get; set; }

        public BaseRequest() { }
        public BaseRequest(T data)
        {
            Data = data;
        }
    }
}
