namespace GerenciamentoUsuarios.DataTransfer.Usuarios.Request;

public class UsuariosEditarRequest
{
    public int Id { get; set; }
    public string? NomeCompleto { get; set; }
    public string? Cpf { get; set; }
    public DateOnly? DataNascimento { get; set; }
    public string? Email { get; set; }
    public string? Senha { get; set; }
}
