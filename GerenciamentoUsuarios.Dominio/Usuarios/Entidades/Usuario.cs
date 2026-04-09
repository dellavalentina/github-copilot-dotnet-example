using GerenciamentoUsuarios.Dominio.libs.Excecoes;
using GerenciamentoUsuarios.Dominio.Usuarios.Enums;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Entidades;

public class Usuario
{
    public virtual int Id { get; protected set; }
    public virtual string NomeCompleto { get; protected set; } = null!;
    public virtual string Cpf { get; protected set; } = null!;
    public virtual DateOnly DataNascimento { get; protected set; }
    public virtual string Email { get; protected set; } = null!;
    public virtual string SenhaHash { get; protected set; } = null!;
    public virtual PerfilUsuario Perfil { get; protected set; }
    public virtual bool Ativo { get; protected set; }

    protected Usuario() { }

    public Usuario(string nomeCompleto, string cpf, DateOnly dataNascimento, string email, string senhaHash, PerfilUsuario perfil)
    {
        SetNomeCompleto(nomeCompleto);
        SetCpf(cpf);
        SetDataNascimento(dataNascimento);
        SetEmail(email);
        SetSenhaHash(senhaHash);
        SetPerfil(perfil);
        Ativo = true;
    }

    public virtual void SetNomeCompleto(string nomeCompleto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto))
            throw new RegraDeNegocioExcecao("Nome completo é obrigatório");
        NomeCompleto = nomeCompleto;
    }

    public virtual void SetCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new RegraDeNegocioExcecao("CPF é obrigatório");
        Cpf = cpf;
    }

    public virtual void SetDataNascimento(DateOnly dataNascimento)
    {
        if (dataNascimento == default)
            throw new RegraDeNegocioExcecao("Data de nascimento é obrigatória");
        DataNascimento = dataNascimento;
    }

    public virtual void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new RegraDeNegocioExcecao("E-mail é obrigatório");
        Email = email;
    }

    public virtual void SetSenhaHash(string senhaHash)
    {
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new RegraDeNegocioExcecao("Senha é obrigatória");
        SenhaHash = senhaHash;
    }

    public virtual void SetPerfil(PerfilUsuario perfil)
    {
        if (!Enum.IsDefined(perfil))
            throw new RegraDeNegocioExcecao("Perfil inválido");
        Perfil = perfil;
    }

    public virtual void Ativar() => Ativo = true;
    public virtual void Desativar() => Ativo = false;
}
