using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.Dominio.libs.UnitOfWork;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.Infra.Comum.UnitOfWork;
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

        // Serviços de Domínio
        services.AddScoped<IUsuariosServicos, UsuariosServicos>();

        // Serviços de Aplicação
        services.AddScoped<IUsuariosAppServico, UsuariosAppServico>();

        return services;
    }
}
