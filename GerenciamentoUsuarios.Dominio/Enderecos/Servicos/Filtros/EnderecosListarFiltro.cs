using GerenciamentoUsuarios.Dominio.libs.Filtros;

namespace GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros;

public class EnderecosListarFiltro : PaginacaoFiltro
{
    public int UsuarioId { get; set; }
    public bool? Ativo { get; set; }
}
