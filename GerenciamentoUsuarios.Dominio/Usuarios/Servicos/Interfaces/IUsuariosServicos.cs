using GerenciamentoUsuarios.Dominio.libs.Consultas;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces;

public interface IUsuariosServicos
{
    Task<Usuario> InserirAsync(UsuariosInserirComando comando, CancellationToken ct = default);
    Task<Usuario> EditarAsync(UsuariosEditarComando comando, CancellationToken ct = default);
    Task ExcluirAsync(int id, CancellationToken ct = default);
    Task ReativarAsync(int id, CancellationToken ct = default);
    Usuario Recuperar(int id);
    PaginacaoConsulta<Usuario> Listar(UsuariosListarFiltro filtro);
}
