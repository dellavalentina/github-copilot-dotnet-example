using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoUsuarios.Infra.Contexto;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Endereco> Enderecos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
