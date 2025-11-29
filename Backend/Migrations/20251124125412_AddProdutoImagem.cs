using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIGA_PET.Migrations
{
    /// <inheritdoc />
    public partial class AddProdutoImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVendas_Produtos_ProdutoId",
                table: "ItemVendas");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemVendas_Servicos_ServicoId",
                table: "ItemVendas");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemVendas_Vendas_VendaId",
                table: "ItemVendas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemVendas",
                table: "ItemVendas");

            migrationBuilder.RenameTable(
                name: "ItemVendas",
                newName: "ItensVenda");

            migrationBuilder.RenameIndex(
                name: "IX_ItemVendas_VendaId",
                table: "ItensVenda",
                newName: "IX_ItensVenda_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemVendas_ServicoId",
                table: "ItensVenda",
                newName: "IX_ItensVenda_ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemVendas_ProdutoId",
                table: "ItensVenda",
                newName: "IX_ItensVenda_ProdutoId");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Fornecedores",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Fornecedores",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensVenda",
                table: "ItensVenda",
                column: "ItemVendaId");

            migrationBuilder.CreateTable(
                name: "ProdutoImagens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoImagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoImagens_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoImagens_ProdutoId",
                table: "ProdutoImagens",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVenda_Produtos_ProdutoId",
                table: "ItensVenda",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVenda_Servicos_ServicoId",
                table: "ItensVenda",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "ServicoId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVenda_Vendas_VendaId",
                table: "ItensVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "VendaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensVenda_Produtos_ProdutoId",
                table: "ItensVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensVenda_Servicos_ServicoId",
                table: "ItensVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensVenda_Vendas_VendaId",
                table: "ItensVenda");

            migrationBuilder.DropTable(
                name: "ProdutoImagens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensVenda",
                table: "ItensVenda");

            migrationBuilder.DropColumn(
                name: "QuantidadeEstoque",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Fornecedores");

            migrationBuilder.RenameTable(
                name: "ItensVenda",
                newName: "ItemVendas");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVenda_VendaId",
                table: "ItemVendas",
                newName: "IX_ItemVendas_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVenda_ServicoId",
                table: "ItemVendas",
                newName: "IX_ItemVendas_ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVenda_ProdutoId",
                table: "ItemVendas",
                newName: "IX_ItemVendas_ProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemVendas",
                table: "ItemVendas",
                column: "ItemVendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVendas_Produtos_ProdutoId",
                table: "ItemVendas",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVendas_Servicos_ServicoId",
                table: "ItemVendas",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "ServicoId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVendas_Vendas_VendaId",
                table: "ItemVendas",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "VendaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
