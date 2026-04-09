using GerenciamentoUsuarios.Dominio.libs.Filtros;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros;

public class UsuariosListarFiltro : PaginacaoFiltro
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cpf { get; set; }
    public bool? Ativo { get; set; }
}
