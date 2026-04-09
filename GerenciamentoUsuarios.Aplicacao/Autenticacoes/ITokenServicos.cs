using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;

namespace GerenciamentoUsuarios.Aplicacao.Autenticacoes;

public interface ITokenServicos
{
    string GerarToken(Usuario usuario);
}
