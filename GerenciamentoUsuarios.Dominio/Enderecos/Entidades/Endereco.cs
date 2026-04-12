using GerenciamentoUsuarios.Dominio.libs.Excecoes;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;

namespace GerenciamentoUsuarios.Dominio.Enderecos.Entidades;

public class Endereco
{
    public virtual int Id { get; protected set; }
    public virtual int UsuarioId { get; protected set; }
    public virtual string Cep { get; protected set; } = null!;
    public virtual string Logradouro { get; protected set; } = null!;
    public virtual string Numero { get; protected set; } = null!;
    public virtual string? Complemento { get; protected set; }
    public virtual string Bairro { get; protected set; } = null!;
    public virtual string Cidade { get; protected set; } = null!;
    public virtual string Estado { get; protected set; } = null!;
    public virtual bool Principal { get; protected set; }
    public virtual bool Ativo { get; protected set; }
    public virtual Usuario Usuario { get; protected set; } = null!;

    protected Endereco() { }

    public Endereco(int usuarioId, string cep, string logradouro, string numero, string? complemento, string bairro, string cidade, string estado, bool principal)
    {
        UsuarioId = usuarioId;
        SetCep(cep);
        SetLogradouro(logradouro);
        SetNumero(numero);
        SetComplemento(complemento);
        SetBairro(bairro);
        SetCidade(cidade);
        SetEstado(estado);
        Principal = principal;
        Ativo = true;
    }

    public virtual void SetCep(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep))
            throw new RegraDeNegocioExcecao("CEP é obrigatório");
        Cep = cep;
    }

    public virtual void SetLogradouro(string logradouro)
    {
        if (string.IsNullOrWhiteSpace(logradouro))
            throw new RegraDeNegocioExcecao("Logradouro é obrigatório");
        Logradouro = logradouro;
    }

    public virtual void SetNumero(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new RegraDeNegocioExcecao("Número é obrigatório");
        Numero = numero;
    }

    public virtual void SetComplemento(string? complemento)
    {
        Complemento = complemento;
    }

    public virtual void SetBairro(string bairro)
    {
        if (string.IsNullOrWhiteSpace(bairro))
            throw new RegraDeNegocioExcecao("Bairro é obrigatório");
        Bairro = bairro;
    }

    public virtual void SetCidade(string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            throw new RegraDeNegocioExcecao("Cidade é obrigatória");
        Cidade = cidade;
    }

    public virtual void SetEstado(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            throw new RegraDeNegocioExcecao("Estado é obrigatório");
        Estado = estado;
    }

    public virtual void DefinirComoPrincipal() => Principal = true;
    public virtual void RemoverPrincipal() => Principal = false;
    public virtual void Ativar() => Ativo = true;
    public virtual void Desativar() => Ativo = false;
}
