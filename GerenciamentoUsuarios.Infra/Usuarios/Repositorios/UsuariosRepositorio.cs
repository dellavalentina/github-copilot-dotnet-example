using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;
using GerenciamentoUsuarios.Infra.Comum.Repositorios;
using GerenciamentoUsuarios.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoUsuarios.Infra.Usuarios.Repositorios;

public class UsuariosRepositorio : RepositorioBase<Usuario>, IUsuariosRepositorio
{
    public UsuariosRepositorio(AppDbContext context) : base(context) { }

    public async Task<bool> ExisteEmailAsync(string email, int? ignorarId, CancellationToken ct = default)
    {
        var query = Query().Where(u => u.Email == email);

        if (ignorarId.HasValue)
            query = query.Where(u => u.Id != ignorarId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task<bool> ExisteCpfAsync(string cpf, int? ignorarId, CancellationToken ct = default)
    {
        var query = Query().Where(u => u.Cpf == cpf);

        if (ignorarId.HasValue)
            query = query.Where(u => u.Id != ignorarId.Value);

        return await query.AnyAsync(ct);
    }
}
