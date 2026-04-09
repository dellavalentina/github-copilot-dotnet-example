using GerenciamentoUsuarios.Dominio.libs.Repositorios;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;

public interface IUsuariosRepositorio : IRepositorioBase<Usuario>
{
    Task<bool> ExisteEmailAsync(string email, int? ignorarId, CancellationToken ct = default);
    Task<bool> ExisteCpfAsync(string cpf, int? ignorarId, CancellationToken ct = default);
}
