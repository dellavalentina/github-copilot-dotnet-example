using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.Enderecos.Repositorios;
using GerenciamentoUsuarios.Infra.Comum.Repositorios;
using GerenciamentoUsuarios.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoUsuarios.Infra.Enderecos.Repositorios;

public class EnderecosRepositorio : RepositorioBase<Endereco>, IEnderecosRepositorio
{
    public EnderecosRepositorio(AppDbContext context) : base(context) { }

    public async Task<Endereco?> RecuperarPrincipalPorUsuarioIdAsync(int usuarioId, CancellationToken ct = default)
    {
        return await Query()
            .Where(e => e.UsuarioId == usuarioId && e.Principal && e.Ativo)
            .FirstOrDefaultAsync(ct);
    }
}
