using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.Enderecos.Repositorios;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.Dominio.libs.Consultas;
using GerenciamentoUsuarios.Dominio.libs.Excecoes;
using GerenciamentoUsuarios.Dominio.Usuarios.Repositorios;

namespace GerenciamentoUsuarios.Dominio.Enderecos.Servicos;

public class EnderecosServicos : IEnderecosServicos
{
    private readonly IEnderecosRepositorio _enderecosRepositorio;
    private readonly IUsuariosRepositorio _usuariosRepositorio;

    public EnderecosServicos(IEnderecosRepositorio enderecosRepositorio, IUsuariosRepositorio usuariosRepositorio)
    {
        _enderecosRepositorio = enderecosRepositorio;
        _usuariosRepositorio = usuariosRepositorio;
    }

    public async Task<Endereco> InserirAsync(EnderecosInserirComando comando, CancellationToken ct = default)
    {
        var usuario = _usuariosRepositorio.Recuperar(comando.UsuarioId);
        ValidarRegistroNaoFoiEncontrado.Validar(usuario);

        if (!usuario!.Ativo)
            throw new RegraDeNegocioExcecao("Usuário está desativado");

        var principal = await _enderecosRepositorio.RecuperarPrincipalPorUsuarioIdAsync(comando.UsuarioId, ct);
        var ehPrimeiro = principal is null;

        var endereco = new Endereco(
            comando.UsuarioId,
            comando.Cep,
            comando.Logradouro,
            comando.Numero,
            comando.Complemento,
            comando.Bairro,
            comando.Cidade,
            comando.Estado,
            ehPrimeiro);

        await _enderecosRepositorio.InserirAsync(endereco, ct);

        return endereco;
    }

    public async Task<Endereco> EditarAsync(EnderecosEditarComando comando, CancellationToken ct = default)
    {
        var endereco = _enderecosRepositorio.Recuperar(comando.Id);
        ValidarRegistroNaoFoiEncontrado.Validar(endereco);

        if (endereco!.UsuarioId != comando.UsuarioId)
            throw new RegraDeNegocioExcecao("Endereço não pertence ao usuário");

        if (!endereco.Ativo)
            throw new RegraDeNegocioExcecao("Endereço está desativado");

        if (comando.Cep is not null)
            endereco.SetCep(comando.Cep);

        if (comando.Logradouro is not null)
            endereco.SetLogradouro(comando.Logradouro);

        if (comando.Numero is not null)
            endereco.SetNumero(comando.Numero);

        if (comando.Complemento is not null)
            endereco.SetComplemento(comando.Complemento);

        if (comando.Bairro is not null)
            endereco.SetBairro(comando.Bairro);

        if (comando.Cidade is not null)
            endereco.SetCidade(comando.Cidade);

        if (comando.Estado is not null)
            endereco.SetEstado(comando.Estado);

        await _enderecosRepositorio.EditarAsync(endereco, ct);

        return endereco;
    }

    public async Task ExcluirAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        var endereco = _enderecosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(endereco);

        if (endereco!.UsuarioId != usuarioId)
            throw new RegraDeNegocioExcecao("Endereço não pertence ao usuário");

        endereco.Desativar();

        if (endereco.Principal)
        {
            endereco.RemoverPrincipal();

            var outroEndereco = _enderecosRepositorio
                .Query()
                .Where(e => e.UsuarioId == usuarioId && e.Ativo && e.Id != id)
                .FirstOrDefault();

            if (outroEndereco is not null)
            {
                outroEndereco.DefinirComoPrincipal();
                await _enderecosRepositorio.EditarAsync(outroEndereco, ct);
            }
        }

        await _enderecosRepositorio.EditarAsync(endereco, ct);
    }

    public async Task DefinirPrincipalAsync(int id, int usuarioId, CancellationToken ct = default)
    {
        var endereco = _enderecosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(endereco);

        if (endereco!.UsuarioId != usuarioId)
            throw new RegraDeNegocioExcecao("Endereço não pertence ao usuário");

        if (!endereco.Ativo)
            throw new RegraDeNegocioExcecao("Endereço está desativado");

        if (endereco.Principal)
            return;

        var principalAtual = await _enderecosRepositorio.RecuperarPrincipalPorUsuarioIdAsync(usuarioId, ct);

        if (principalAtual is not null)
        {
            principalAtual.RemoverPrincipal();
            await _enderecosRepositorio.EditarAsync(principalAtual, ct);
        }

        endereco.DefinirComoPrincipal();
        await _enderecosRepositorio.EditarAsync(endereco, ct);
    }

    public Endereco Recuperar(int id, int usuarioId)
    {
        var endereco = _enderecosRepositorio.Recuperar(id);
        ValidarRegistroNaoFoiEncontrado.Validar(endereco);

        if (endereco!.UsuarioId != usuarioId)
            throw new RegraDeNegocioExcecao("Endereço não pertence ao usuário");

        return endereco;
    }

    public PaginacaoConsulta<Endereco> Listar(EnderecosListarFiltro filtro)
    {
        var query = _enderecosRepositorio.Query()
            .Where(e => e.UsuarioId == filtro.UsuarioId);

        if (filtro.Ativo.HasValue)
            query = query.Where(e => e.Ativo == filtro.Ativo.Value);

        var total = query.Count();

        var lista = query
            .Skip((filtro.Pg - 1) * filtro.Qt)
            .Take(filtro.Qt)
            .ToList();

        return new PaginacaoConsulta<Endereco>
        {
            Lista = lista,
            Total = total,
            Qt = filtro.Qt,
            Pg = filtro.Pg
        };
    }
}
