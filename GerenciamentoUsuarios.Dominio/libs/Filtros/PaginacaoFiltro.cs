namespace GerenciamentoUsuarios.Dominio.libs.Filtros;

public class PaginacaoFiltro
{
    public int Qt { get; set; } = 10;
    public int Pg { get; set; } = 1;
    public string? CpOrd { get; set; }
    public string? TpOrd { get; set; }
}
