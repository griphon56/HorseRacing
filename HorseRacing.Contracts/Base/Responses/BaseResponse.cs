using HorseRacing.Contracts.Base.Dto;

namespace HorseRacing.Contracts.Base.Responses
{
    /// <summary>
    /// Базовая модель ответа
    /// </summary>
    public class BaseResponse<T>
        where T : BaseDto, new()
    {
        public BaseResponse() { }
        public BaseResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; } = new();
    }

    /// <summary>
    /// Базовая модель ответа. В ответе список данных
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseListResponse<T>
        where T : BaseDto, new()
    {
        public List<T> DataValues { get; set; } = new();

        public BaseListResponse() { }

        public BaseListResponse(List<T> data)
        {
            DataValues = data;
        }
    }
}
