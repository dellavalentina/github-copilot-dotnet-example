using AutoMapper;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciamentoUsuarios.Ioc;

public static class ConfiguracoesAutoMapper
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<UsuariosProfile>());

        return services;
    }
}
