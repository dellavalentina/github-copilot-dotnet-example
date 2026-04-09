using GerenciamentoUsuarios.Dominio.libs.Consultas;
using GerenciamentoUsuarios.Dominio.libs.Excecoes;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Interfaces;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Servicos;

public class UsuariosServicos : IUsuariosServicos
{
    private readonly IUsuariosRepositorio _usuariosRepositorio;

    public UsuariosServicos(IUsuariosRepositorio usuariosRepositorio)
    {
        _usuariosRepositorio = usuariosRepositorio;
    }

    public async Task<Usuario> InserirAsync(UsuariosInserirComando comando, CancellationToken ct = default)
    {
        if (await _usuariosRepositorio.ExisteEmailAsync(comando.Email, null, ct))
            throw new RegraDeNegocioExcecao("O e-mail informado já está em uso");

        if (await _usuariosRepositorio.ExisteCpfAsync(comando.Cpf, null, ct))
            throw new RegraDeNegocioExcecao("O CPF informado já está em uso");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(comando.Senha);

        var usuario = new Usuario(
            comando.NomeCompleto,
            comando.Cpf,
            comando.DataNascimento,
            comando.Email,
            senhaHash,
            comando.Perfil);

        await _usuariosRepositorio.InserirAsync(usuario, ct);

        return usuario;
    }

    public async Task<Usuario> EditarAsync(UsuariosEditarComando comando, CancellationToken ct = default)
    {
        var usuario = _usuariosRepositorio.Recuperar(comando.Id);
        ValidarRegistroNaoFoiEncontrado.Validar(usuario);

        if (comando.Email is not null)
        {
            if (await _usuariosRepositorio.ExisteEmailAsync(comando.Email, comando.Id, ct))
                throw new RegraDeNegocioExcecao("O e-mail informado já está em uso");
            usuario!.SetEmail(comando.Email);
        }

        if (comando.Cpf is not null)
        {
            if (await _usuariosRepositorio.ExisteCpfAsync(comando.Cpf, comando.Id, ct))
                throw new RegraDeNegocioExcecao("O CPF informado já está em uso");
            usuario!.SetCpf(comando.Cpf);
        }

        if (comando.NomeCompleto is not null)
            usuario!.SetNomeCompleto(comando.NomeCompleto);

        if (comando.DataNascimento is not null)
            usuario!.SetDataNascimento(comando.DataNascimento.Value);

        if (comando.Senha is not null)
        {
            var senhaHash = BCrypt.Net.BCrypt.HashPassword(comando.Senha);
            usuario!.SetSenhaHash(senhaHash);
        }

        await _usuariosRepositorio.EditarAsync(usuario!, ct);

        return usuario!;
    }

    public async Task ExcluirAsync(int id, CancellationToken ct = default)
    {
        var usuario = _usuariosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(usuario);

        usuario!.Desativar();

        await _usuariosRepositorio.EditarAsync(usuario, ct);
    }

    public async Task ReativarAsync(int id, CancellationToken ct = default)
    {
        var usuario = _usuariosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(usuario);

        usuario!.Ativar();

        await _usuariosRepositorio.EditarAsync(usuario, ct);
    }

    public Usuario Recuperar(int id)
    {
        var usuario = _usuariosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(usuario);
        return usuario!;
    }

    public PaginacaoConsulta<Usuario> Listar(UsuariosListarFiltro filtro)
    {
        var query = _usuariosRepositorio.Query();

        if (!string.IsNullOrWhiteSpace(filtro.Nome))
            query = query.Where(u => u.NomeCompleto.ToLower().Contains(filtro.Nome.ToLower()));

        if (!string.IsNullOrWhiteSpace(filtro.Email))
            query = query.Where(u => u.Email.ToLower().Contains(filtro.Email.ToLower()));

        if (!string.IsNullOrWhiteSpace(filtro.Cpf))
            query = query.Where(u => u.Cpf == filtro.Cpf);

        if (filtro.Ativo.HasValue)
            query = query.Where(u => u.Ativo == filtro.Ativo.Value);

        var total = query.Count();

        var lista = query
            .Skip((filtro.Pg - 1) * filtro.Qt)
            .Take(filtro.Qt)
            .ToList();

        return new PaginacaoConsulta<Usuario>
        {
            Lista = lista,
            Total = total,
            Qt = filtro.Qt,
            Pg = filtro.Pg
        };
    }
}
