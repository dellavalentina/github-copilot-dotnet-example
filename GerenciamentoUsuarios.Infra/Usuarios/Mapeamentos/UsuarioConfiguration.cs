using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciamentoUsuarios.Infra.Usuarios.Mapeamentos;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.NomeCompleto)
            .HasColumnName("nome_completo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Cpf)
            .HasColumnName("cpf")
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(x => x.DataNascimento)
            .HasColumnName("data_nascimento")
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.SenhaHash)
            .HasColumnName("senha_hash")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Perfil)
            .HasColumnName("perfil")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.Cpf).IsUnique();
    }
}
