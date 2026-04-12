using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.libs.Repositorios;

namespace GerenciamentoUsuarios.Dominio.Enderecos.Repositorios;

public interface IEnderecosRepositorio : IRepositorioBase<Endereco>
{
    Task<Endereco?> RecuperarPrincipalPorUsuarioIdAsync(int usuarioId, CancellationToken ct = default);
}
