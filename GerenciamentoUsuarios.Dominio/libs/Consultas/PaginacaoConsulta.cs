namespace GerenciamentoUsuarios.Dominio.libs.Consultas;

public class PaginacaoConsulta<T>
{
    public IEnumerable<T> Lista { get; set; } = [];
    public int Total { get; set; }
    public int Qt { get; set; }
    public int Pg { get; set; }
}
