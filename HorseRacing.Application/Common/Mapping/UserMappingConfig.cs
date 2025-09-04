using HorseRacing.Application.RequestHandlers.UserHandlers.Commands.UpdateUser;
using HorseRacing.Application.RequestHandlers.UserHandlers.Common;
using HorseRacing.Contracts.Models.User.Requests.UpdateUser;
using HorseRacing.Contracts.Models.User.Responses.GetAccountBalance;
using HorseRacing.Contracts.Models.User.Responses.GetUser;
using HorseRacing.Domain.UserAggregate.ValueObjects;
using Mapster;

namespace HorseRacing.Application.Common.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GetUserResult, GetUserResponse>()
                .Map(dest => dest.Data.UserName, src => src.UserName)
                .Map(dest => dest.Data.FirstName, src => src.FirstName)
                .Map(dest => dest.Data.LastName, src => src.LastName)
                .Map(dest => dest.Data.Email, src => src.Email)
                .Map(dest => dest.Data.Phone, src => src.Phone);

            config.NewConfig<UpdateUserRequest, UpdateUserCommand>()
                .Map(dest => dest.UserId, src => UserId.Create(src.Data.UserId))
                .Map(dest => dest.Password, src => src.Data.Password)
                .Map(dest => dest.UserName, src => src.Data.UserName)
                .Map(dest => dest.FirstName, src => src.Data.FirstName)
                .Map(dest => dest.LastName, src => src.Data.LastName)
                .Map(dest => dest.Email, src => src.Data.Email)
                .Map(dest => dest.Phone, src => src.Data.Phone);

            config.NewConfig<GetAccountBalanceResult, GetAccountBalanceResponse>()
                .Map(dest => dest.Data.UserId, src => src.UserId.Value)
                .Map(dest => dest.Data.Balance, src => src.Balance);
        }
    }
}