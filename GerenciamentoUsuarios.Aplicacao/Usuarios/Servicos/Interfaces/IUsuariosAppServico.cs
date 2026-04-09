using GerenciamentoUsuarios.DataTransfer.Usuarios.Request;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Response;
using GerenciamentoUsuarios.Dominio.libs.Consultas;

namespace GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces;

public interface IUsuariosAppServico
{
    Task<UsuariosResponse> InserirAsync(UsuariosInserirRequest request, CancellationToken ct = default);
    Task<UsuariosResponse> EditarAsync(UsuariosEditarRequest request, CancellationToken ct = default);
    Task ExcluirAsync(int id, int usuarioAutenticadoId, CancellationToken ct = default);
    Task ReativarAsync(int id, CancellationToken ct = default);
    UsuariosResponse Recuperar(int id);
    PaginacaoConsulta<UsuariosResponse> Listar(UsuariosListarRequest request);
    Task<string> LoginAsync(UsuariosLoginRequest request, CancellationToken ct = default);
}
