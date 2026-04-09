using AutoMapper;
using GerenciamentoUsuarios.Aplicacao.Autenticacoes;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Request;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Response;
using GerenciamentoUsuarios.Dominio.libs.Consultas;
using GerenciamentoUsuarios.Dominio.libs.Excecoes;
using GerenciamentoUsuarios.Dominio.libs.UnitOfWork;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces;

namespace GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos;

public class UsuariosAppServico : IUsuariosAppServico
{
    private readonly IUsuariosServicos _usuariosServicos;
    private readonly IUsuariosRepositorio _usuariosRepositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ITokenServicos _tokenServicos;

    public UsuariosAppServico(
        IUsuariosServicos usuariosServicos,
        IUsuariosRepositorio usuariosRepositorio,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenServicos tokenServicos)
    {
        _usuariosServicos = usuariosServicos;
        _usuariosRepositorio = usuariosRepositorio;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenServicos = tokenServicos;
    }

    public async Task<UsuariosResponse> InserirAsync(UsuariosInserirRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<UsuariosInserirComando>(request);
            var entidade = await _usuariosServicos.InserirAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UsuariosResponse>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<UsuariosResponse> EditarAsync(UsuariosEditarRequest request, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            var comando = _mapper.Map<UsuariosEditarComando>(request);
            var entidade = await _usuariosServicos.EditarAsync(comando, ct);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<UsuariosResponse>(entidade);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ExcluirAsync(int id, int usuarioAutenticadoId, CancellationToken ct = default)
    {
        if (id != usuarioAutenticadoId)
            throw new RegraDeNegocioExcecao("Não é permitido desativar a conta de outro usuário");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _usuariosServicos.ExcluirAsync(id, ct);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task ReativarAsync(int id, CancellationToken ct = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _usuariosServicos.ReativarAsync(id, ct);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public UsuariosResponse Recuperar(int id)
    {
        var entidade = _usuariosServicos.Recuperar(id);
        return _mapper.Map<UsuariosResponse>(entidade);
    }

    public PaginacaoConsulta<UsuariosResponse> Listar(UsuariosListarRequest request)
    {
        var filtro = _mapper.Map<UsuariosListarFiltro>(request);
        var resultado = _usuariosServicos.Listar(filtro);

        return new PaginacaoConsulta<UsuariosResponse>
        {
            Lista = _mapper.Map<IEnumerable<UsuariosResponse>>(resultado.Lista),
            Total = resultado.Total,
            Qt = resultado.Qt,
            Pg = resultado.Pg
        };
    }

    public async Task<string> LoginAsync(UsuariosLoginRequest request, CancellationToken ct = default)
    {
        var usuario = await _usuariosRepositorio.RecuperarAsync(u => u.Email == request.Email, ct);

        if (usuario is null)
            throw new RegraDeNegocioExcecao("Credenciais inválidas");

        if (!usuario.Ativo)
            throw new RegraDeNegocioExcecao("Conta desativada");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new RegraDeNegocioExcecao("Credenciais inválidas");

        return _tokenServicos.GerarToken(usuario);
    }
}
