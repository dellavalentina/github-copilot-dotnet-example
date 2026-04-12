using GerenciamentoUsuarios.Dominio.libs.Filtros;

namespace GerenciamentoUsuarios.DataTransfer.Enderecos.Request;

public class EnderecosListarRequest : PaginacaoFiltro
{
    public bool? Ativo { get; set; }
}
