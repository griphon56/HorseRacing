using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Commands.RegistrationUsingForms;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Common;
using HorseRacing.Application.RequestHandlers.AuthenticationHandlers.Queries.LoginUsingForms;
using HorseRacing.Contracts.Models.Authentication.Dtos;
using HorseRacing.Contracts.Models.Authentication.Responses;
using Mapster;

namespace HorseRacing.Application.Common.Mapping
{
    /// <summary>
    /// Конфигурация сопоставления аутентификации.
    /// </summary>
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegistrationRequestDto, RegistrationCommand>();
            config.NewConfig<LoginDto, LoginQuery>();

            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest.Data.Token, src => src.Token)
                .Map(dest => dest.Data.Id, src => src.User!.Id.Value)
                .Map(dest => dest.Data.Email, src => src.User!.Email ?? "")
                .Map(dest => dest.Data.UserName, src => src.User!.UserName)
                .Map(dest => dest.Data.FirstName, src => src.User!.FirstName)
                .Map(dest => dest.Data.LastName, src => src.User!.LastName);
        }
    }
}
