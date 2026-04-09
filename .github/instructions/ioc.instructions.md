---
applyTo: "**/Configuracoes*.cs"
---
# Padrões da Camada IoC (<Projeto>.Ioc)

## Responsabilidade
Configurar injeção de dependências, Entity Framework Core e AutoMapper.

## Estrutura de Pastas
```
<Projeto>.Ioc/
├── ConfiguracoesInjecoesDependencia.cs
├── ConfiguracoesDbContext.cs
├── ConfiguracoesAutoMapper.cs
├── ConfiguracoesQuartz.cs
└── ConfiguracoesSettings.cs
```

## Padrões de Código

### Registro de Dependências
```csharp
namespace <Projeto>.Ioc;

public static class ConfiguracoesInjecoesDependencia
{
    public static IServiceCollection AddInjecoesDependencia(this IServiceCollection services)
    {
        // Repositórios
        services.AddScoped<I<Feature>Repositorio, <Feature>Repositorio>();

        // Serviços de Domínio
        services.AddScoped<I<Feature>Servicos, <Feature>Servicos>();

        // Serviços de Aplicação
        services.AddScoped<I<Feature>AppServico, <Feature>AppServico>();

        return services;
    }
}
```

### Configuração do AutoMapper
```csharp
namespace <Projeto>.Ioc;

public static class ConfiguracoesAutoMapper
{
    public static IServiceCollection AddAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton(new MapperConfiguration(config =>
        {
            config.AddProfile<<Feature>Profile>();
            // Adicionar novos profiles aqui
        }).CreateMapper());

        return services;
    }
}
```

## Ordem de Registro (manter consistente)
1. Repositórios
2. Serviços de Domínio
3. Serviços de Aplicação
4. Serviços de Infraestrutura (e-mail, S3, etc.)

## Ciclo de Vida
| Tipo | Ciclo | Quando Usar |
|------|-------|-------------|
| `AddScoped` | Por request | Repositórios, Serviços, AppServicos |
| `AddSingleton` | Único | AutoMapper, Configurações |
| `AddTransient` | Cada injeção | Factories |

## Checklist ao adicionar nova feature
```
[ ] services.AddScoped<I<Feature>Repositorio, <Feature>Repositorio>()
[ ] services.AddScoped<I<Feature>Servicos, <Feature>Servicos>()
[ ] services.AddScoped<I<Feature>AppServico, <Feature>AppServico>()
[ ] config.AddProfile<<Feature>Profile>()
[ ] DbSet<T> no AppDbContext
[ ] dotnet ef migrations add <Nome>
```

## Regras
- ✅ Toda interface DEVE ser registrada aqui — ausência gera `InvalidOperationException` ao ativar controllers
- ✅ Manter ordem de registro consistente
- ❌ NÃO usar `AddSingleton` para serviços que dependem de `AppDbContext`
