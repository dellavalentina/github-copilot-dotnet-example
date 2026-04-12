using AutoMapper;
using GerenciamentoUsuarios.Aplicacao.Enderecos.Profiles;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciamentoUsuarios.Ioc;

public static class ConfiguracoesAutoMapper
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<UsuariosProfile>();
            cfg.AddProfile<EnderecosProfile>();
        });

        return services;
    }
}
