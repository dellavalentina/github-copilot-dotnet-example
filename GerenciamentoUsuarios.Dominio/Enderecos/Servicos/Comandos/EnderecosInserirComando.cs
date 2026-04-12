namespace GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos;

public class EnderecosInserirComando
{
    public int UsuarioId { get; set; }
    public string Cep { get; set; } = null!;
    public string Logradouro { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;
}
