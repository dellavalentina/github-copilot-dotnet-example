using GerenciamentoUsuarios.Dominio.libs.Filtros;

namespace GerenciamentoUsuarios.DataTransfer.Usuarios.Request;

public class UsuariosListarRequest : PaginacaoFiltro
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public string? Cpf { get; set; }
    public bool? Ativo { get; set; }
}
