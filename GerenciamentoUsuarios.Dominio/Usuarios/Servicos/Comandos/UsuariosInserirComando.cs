using GerenciamentoUsuarios.Dominio.Usuarios.Enums;

namespace GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos;

public class UsuariosInserirComando
{
    public string NomeCompleto { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public DateOnly DataNascimento { get; set; }
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public PerfilUsuario Perfil { get; set; }
}
