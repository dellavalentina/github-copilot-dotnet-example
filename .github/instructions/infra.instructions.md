---
applyTo: "**/Repositorios/*.cs,**/Mapeamentos/*.cs,**/Contexto/*.cs,**/*Configuration.cs"
---

# Padrões da Camada Infraestrutura (<Projeto>.Infra)

## Responsabilidade

Implementar repositórios, mapeamentos EF Core, DbContext e serviços de infraestrutura (e-mail, S3, etc).

## Estrutura de Pastas

```
<Projeto>.Infra/
├── Comum/
│   └── Repositorios/
│       └── RepositorioBase.cs
├── Contexto/
│   └── AppDbContext.cs
├── Migrations/
└── <Feature>/
    ├── Mapeamentos/
    │   └── <Entidade>Configuration.cs
    ├── Repositorios/
    │   └── <Feature>Repositorio.cs
    └── Servicos/
        └── <Integracao>Servico.cs
```

## Nomenclatura

| Elemento              | Padrão                    | Exemplo                |
| --------------------- | ------------------------- | ---------------------- |
| Repositório           | `<Feature>Repositorio`    | `UsuariosRepositorio`  |
| Mapeamento            | `<Entidade>Configuration` | `UsuarioConfiguration` |
| Serviço de integração | `<Integracao>Servico`     | `NorteboxServico`      |

## Padrões de Código

### Repositório da Feature

```csharp
namespace <Projeto>.Infra.<Feature>.Repositorios;

public class <Feature>Repositorio : RepositorioBase<<Entidade>>, I<Feature>Repositorio
{
    public <Feature>Repositorio(AppDbContext context) : base(context) { }

    // Métodos específicos da feature, se necessário
}
```

### Mapeamento EF Core (Fluent API)

```csharp
namespace <Projeto>.Infra.<Feature>.Mapeamentos;

public class <Entidade>Configuration : IEntityTypeConfiguration<<Entidade>>
{
    public void Configure(EntityTypeBuilder<<Entidade>> builder)
    {
        builder.ToTable("<tabela_plural>");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.Nome)
            .HasColumnName("nome")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        // Enum (armazenado como int)
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<int>();

        // Relacionamento Many-to-One
        builder.HasOne(x => x.Entidade)
            .WithMany()
            .HasForeignKey("EntidadeId");
    }
}
```

### AppDbContext

```csharp
// Declarar um DbSet<T> por entidade mapeada
public DbSet<<Entidade>> <Entidades> { get; set; }

// Mapeamentos são descobertos automaticamente:
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
// Adicionar novos IEntityTypeConfiguration<T> em Mapeamentos/ — sem registro manual necessário
```

## Métodos herdados de `RepositorioBase<T>`

```csharp
// Síncronos
Inserir(T), Editar(T), Excluir(T)
Recuperar(int id), Recuperar(Expression<Func<T, bool>>)
Query()  // IQueryable<T> para filtros e paginação

// Assíncronos
InserirAsync(T, ct), EditarAsync(T, ct), ExcluirAsync(T, ct)
RecuperarAsync(int id, ct), RecuperarAsync(Expression<Func<T, bool>>, ct)
```

## Regras

- ✅ Reutilizar métodos do `RepositorioBase<T>` — não criar acesso a dados customizado
- ✅ Usar `Query()` para montar queries paginadas/filtradas com LINQ
- ✅ Novos `IEntityTypeConfiguration<T>` são descobertos automaticamente por `ApplyConfigurationsFromAssembly`
- ✅ Toda nova entidade precisa de `DbSet<T>` no `AppDbContext` + Migration EF Core
- ❌ NÃO chamar `SaveChanges` manual fora do `RepositorioBase<T>`
