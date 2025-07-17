using HorseRacing.Application.Base;

namespace HorseRacing.Application.RequestHandlers.GameHandlers.Common
{
    public class CheckPlayerConnectedToGameResult : BaseModelResult
    {
        public bool IsConnected { get; set; }

        public CheckPlayerConnectedToGameResult(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}
