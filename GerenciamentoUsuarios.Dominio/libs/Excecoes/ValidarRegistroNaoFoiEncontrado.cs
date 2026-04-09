namespace GerenciamentoUsuarios.Dominio.libs.Excecoes;

public static class ValidarRegistroNaoFoiEncontrado
{
    public static void Validar(object? entidade)
    {
        if (entidade is null)
            throw new RegraDeNegocioExcecao("Registro não foi encontrado");
    }
}
