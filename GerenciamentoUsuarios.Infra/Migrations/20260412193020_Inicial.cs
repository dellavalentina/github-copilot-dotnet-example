using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciamentoUsuarios.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_completo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    data_nascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    senha_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    perfil = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "enderecos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    logradouro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    complemento = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bairro = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    cidade = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    principal = table.Column<bool>(type: "bit", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enderecos", x => x.id);
                    table.ForeignKey(
                        name: "FK_enderecos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_enderecos_usuario_id",
                table: "enderecos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_cpf",
                table: "usuarios",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enderecos");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
