using GerenciamentoUsuarios.Dominio.Usuarios.Enums;

namespace GerenciamentoUsuarios.DataTransfer.Usuarios.Request;

public class UsuariosInserirRequest
{
    public string NomeCompleto { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public DateOnly DataNascimento { get; set; }
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public PerfilUsuario Perfil { get; set; }
}
