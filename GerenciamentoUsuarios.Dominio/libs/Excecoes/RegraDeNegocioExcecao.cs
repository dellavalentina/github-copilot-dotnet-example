namespace GerenciamentoUsuarios.Dominio.libs.Excecoes;

public class RegraDeNegocioExcecao : Exception
{
    public RegraDeNegocioExcecao(string mensagem) : base(mensagem) { }
}
