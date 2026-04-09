---
applyTo: "**/*Job.cs,**/ConfiguracoesQuartz.cs"
---
# Padrões da Camada Jobs (<Projeto>.Jobs)

## Responsabilidade
Background jobs Quartz.NET executados no processo da API via `AddQuartzHostedService`.

## Nomenclatura
- Classes: `<Acao/Descricao>Job` — ex: `ColetaMD50Job`, `ConsolidacaoDiariaJob`
- Namespace: `<Projeto>.Jobs.<Feature>.Jobs`

## Implementação

### Estrutura base
```csharp
namespace <Projeto>.Jobs.<Feature>.Jobs;

[DisallowConcurrentExecution] // obrigatório para jobs periódicos
public class <Descricao>Job : IJob
{
    private readonly I<Feature>Servicos _<feature>Servicos;
    private readonly ILogger<<Descricao>Job> _logger;

    public <Descricao>Job(I<Feature>Servicos <feature>Servicos, ILogger<<Descricao>Job> logger)
    {
        _<feature>Servicos = <feature>Servicos;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Iniciando {Job}", nameof(<Descricao>Job));
        var ct = context.CancellationToken;

        // Iterar por itens com try/catch individual (não interromper o loop por falha)
        foreach (var item in itens)
        {
            try
            {
                await _<feature>Servicos.ProcessarAsync(item, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar item {Id}", item.Id);
            }
        }

        _logger.LogInformation("Concluído {Job}", nameof(<Descricao>Job));
    }
}
```

### Agendamento (ConfiguracoesQuartz.cs)
```csharp
// Job periódico com cron
q.AddJob<ColetaMD50Job>(opts => opts.WithIdentity("ColetaMD50Job"));
q.AddTrigger(opts => opts
    .ForJob("ColetaMD50Job")
    .WithCronSchedule("0 */15 * * * ?"));  // a cada 15 minutos

// Job manual (sem trigger automático)
q.AddJob<SincronizacaoJob>(opts => opts
    .WithIdentity("SincronizacaoJob")
    .StoreDurably());
```

## Regras
- ✅ `[DisallowConcurrentExecution]` em todos os jobs periódicos
- ✅ Usar `context.CancellationToken` para cancelamento graceful
- ✅ Logging estruturado com início, progresso e fim
- ✅ Try/catch individual por item em loops — não interromper o loop por falha unitária
- ✅ `IUnitOfWork` para transações explícitas quando necessário
- ✅ Injetar serviços de domínio via construtor — nunca acessar `DbContext` diretamente no job
- ❌ NÃO colocar lógica de negócio no job — delegar para serviços de domínio
- ❌ NÃO usar `Thread.Sleep` — Quartz gerencia o agendamento
