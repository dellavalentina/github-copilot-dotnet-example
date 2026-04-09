using GerenciamentoUsuarios.Dominio.Usuarios.Enums;

namespace GerenciamentoUsuarios.DataTransfer.Usuarios.Response;

public class UsuariosResponse
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public DateOnly DataNascimento { get; set; }
    public string Email { get; set; } = null!;
    public PerfilUsuario Perfil { get; set; }
    public bool Ativo { get; set; }
}
