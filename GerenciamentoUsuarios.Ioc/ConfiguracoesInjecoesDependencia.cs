using GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos;
using GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.Dominio.Enderecos.Repositorios;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.Dominio.libs.UnitOfWork;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.Infra.Comum.UnitOfWork;
using GerenciamentoUsuarios.Infra.Enderecos.Repositorios;
using GerenciamentoUsuarios.Infra.Usuarios.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace GerenciamentoUsuarios.Ioc;

public static class ConfiguracoesInjecoesDependencia
{
    public static IServiceCollection AddInjecoesDependencia(this IServiceCollection services)
    {
        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositórios
        services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
        services.AddScoped<IEnderecosRepositorio, EnderecosRepositorio>();

        // Serviços de Domínio
        services.AddScoped<IUsuariosServicos, UsuariosServicos>();
        services.AddScoped<IEnderecosServicos, EnderecosServicos>();

        // Serviços de Aplicação
        services.AddScoped<IUsuariosAppServico, UsuariosAppServico>();
        services.AddScoped<IEnderecosAppServico, EnderecosAppServico>();

        return services;
    }
}
