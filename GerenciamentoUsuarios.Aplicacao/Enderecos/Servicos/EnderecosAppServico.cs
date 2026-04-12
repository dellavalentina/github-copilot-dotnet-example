using AutoMapper;
using GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Request;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Response;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.Dominio.libs.Consultas;
using GerenciamentoUsuarios.Dominio.libs.UnitOfWork;

namespace GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos;

public class EnderecosAppServico : IEnderecosAppServico
{
    private readonly IEnderecosServicos _enderecosServicos;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnderecosAppServico(IEnderecosServicos enderecosServicos, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _enderecosServicos = enderecosServicos;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EnderecosResponse> InserirAsync(int usuarioId, EnderecosInserirRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<EnderecosInserirComando>(request);
            comando.UsuarioId = usuarioId;
            var entidade = await _enderecosServicos.InserirAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EnderecosResponse>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<EnderecosResponse> EditarAsync(int id, int usuarioId, EnderecosEditarRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<EnderecosEditarComando>(request);
            comando.Id = id;
            comando.UsuarioId = usuarioId;
            var entidade = await _enderecosServicos.EditarAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EnderecosResponse>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ExcluirAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _enderecosServicos.ExcluirAsync(id, usuarioId, ct);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DefinirPrincipalAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _enderecosServicos.DefinirPrincipalAsync(id, usuarioId, ct);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public EnderecosResponse Recuperar(int id, int usuarioId)
    {
        var entidade = _enderecosServicos.Recuperar(id, usuarioId);
        return _mapper.Map<EnderecosResponse>(entidade);
    }

    public PaginacaoConsulta<EnderecosResponse> Listar(int usuarioId, EnderecosListarRequest request)
    {
        var filtro = _mapper.Map<EnderecosListarFiltro>(request);
        filtro.UsuarioId = usuarioId;
        var resultado = _enderecosServicos.Listar(filtro);

        return new PaginacaoConsulta<EnderecosResponse>
        {
            Lista = _mapper.Map<IEnumerable<EnderecosResponse>>(resultado.Lista),
            Total = resultado.Total,
            Qt = resultado.Qt,
            Pg = resultado.Pg
        };
    }
}
