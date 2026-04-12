using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros;
using GerenciamentoUsuarios.Dominio.libs.Consultas;

namespace GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Interfaces;

public interface IEnderecosServicos
{
    Task<Endereco> InserirAsync(EnderecosInserirComando comando, CancellationToken ct = default);
    Task<Endereco> EditarAsync(EnderecosEditarComando comando, CancellationToken ct = default);
    Task ExcluirAsync(int id, int usuarioId, CancellationToken ct = default);
    Task DefinirPrincipalAsync(int id, int usuarioId, CancellationToken ct = default);
    Endereco Recuperar(int id, int usuarioId);
    PaginacaoConsulta<Endereco> Listar(EnderecosListarFiltro filtro);
}
