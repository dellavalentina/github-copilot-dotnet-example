using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciamentoUsuarios.Infra.Enderecos.Mapeamentos;

public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
{
    public void Configure(EntityTypeBuilder<Endereco> builder)
    {
        builder.ToTable("enderecos");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();

        builder.Property(x => x.UsuarioId)
            .HasColumnName("usuario_id")
            .IsRequired();

        builder.Property(x => x.Cep)
            .HasColumnName("cep")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.Logradouro)
            .HasColumnName("logradouro")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Numero)
            .HasColumnName("numero")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Complemento)
            .HasColumnName("complemento")
            .HasMaxLength(255);

        builder.Property(x => x.Bairro)
            .HasColumnName("bairro")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Cidade)
            .HasColumnName("cidade")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Estado)
            .HasColumnName("estado")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(x => x.Principal)
            .HasColumnName("principal")
            .IsRequired();

        builder.Property(x => x.Ativo)
            .HasColumnName("ativo")
            .IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
