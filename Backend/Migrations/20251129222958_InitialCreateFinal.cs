using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGA_PET.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Usuarios_UsuarioId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutores_Usuarios_UsuarioId",
                table: "Tutores");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Usuarios_UsuarioId",
                table: "Funcionarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutores_Usuarios_UsuarioId",
                table: "Tutores",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Usuarios_UsuarioId",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutores_Usuarios_UsuarioId",
                table: "Tutores");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Usuarios_UsuarioId",
                table: "Funcionarios",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tutores_Usuarios_UsuarioId",
                table: "Tutores",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
