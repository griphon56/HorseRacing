using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Base.Responses
{
    /// <summary>
    /// Модель ответа
    /// </summary>
    public class BaseModelResponse<T> : BaseResponse<T>
        where T : BaseModelDto, new()
    {
        public BaseModelResponse() { }
        public BaseModelResponse(T data) : base(data)
        {
            Data = data;
        }
    }
}
