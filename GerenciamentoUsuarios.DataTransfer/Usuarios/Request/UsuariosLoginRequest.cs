namespace GerenciamentoUsuarios.DataTransfer.Usuarios.Request;

public class UsuariosLoginRequest
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}
