using GerenciamentoUsuarios.DataTransfer.Enderecos.Request;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Response;
using GerenciamentoUsuarios.Dominio.libs.Consultas;

namespace GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos.Interfaces;

public interface IEnderecosAppServico
{
    Task<EnderecosResponse> InserirAsync(int usuarioId, EnderecosInserirRequest request, CancellationToken ct = default);
    Task<EnderecosResponse> EditarAsync(int id, int usuarioId, EnderecosEditarRequest request, CancellationToken ct = default);
    Task ExcluirAsync(int id, int usuarioId, CancellationToken ct = default);
    Task DefinirPrincipalAsync(int id, int usuarioId, CancellationToken ct = default);
    EnderecosResponse Recuperar(int id, int usuarioId);
    PaginacaoConsulta<EnderecosResponse> Listar(int usuarioId, EnderecosListarRequest request);
}
