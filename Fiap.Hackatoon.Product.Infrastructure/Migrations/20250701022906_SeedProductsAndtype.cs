using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiap.Hackatoon.Product.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductsAndtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seeding ProductType
            migrationBuilder.InsertData(
                table: "ProductType",
                columns: new[] { "Id", "Name", "Code", "Description", "CreatedAt" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Lanche", "LAN", "Lanches rápidos e sanduíches", DateTime.UtcNow },
                    { Guid.NewGuid(), "Sobremesa", "SOB", "Doces e sobremesas", DateTime.UtcNow },
                    { Guid.NewGuid(), "Bebida", "BEB", "Bebidas em geral", DateTime.UtcNow },
                    { Guid.NewGuid(), "Prato Principal", "PRP", "Refeições principais", DateTime.UtcNow },
                    { Guid.NewGuid(), "Acompanhamento", "ACP", "Acompanhamentos para os pratos", DateTime.UtcNow }
                });

            // Define IDs for ProductType
            var lancheId = Guid.NewGuid();
            var bebidaId = Guid.NewGuid();
            var sobremesaId = Guid.NewGuid();
            var pratoPrincipalId = Guid.NewGuid();
            var acompanhamentoId = Guid.NewGuid();

            migrationBuilder.InsertData(
                table: "ProductType",
                columns: new[] { "Id", "Name", "Code", "Description", "CreatedAt" },
                values: new object[,]
                {
                    { lancheId, "Lanche", "LAN", "Lanches rápidos e sanduíches", DateTime.UtcNow },
                    { sobremesaId, "Sobremesa", "SOB", "Doces e sobremesas", DateTime.UtcNow },
                    { bebidaId, "Bebida", "BEB", "Bebidas em geral", DateTime.UtcNow },
                    { pratoPrincipalId, "Prato Principal", "PRP", "Refeições principais", DateTime.UtcNow },
                    { acompanhamentoId, "Acompanhamento", "ACP", "Acompanhamentos para os pratos", DateTime.UtcNow },
                    { Guid.NewGuid(), "Salada", "SAL", "Saladas frescas e leves", DateTime.UtcNow },
                    { Guid.NewGuid(), "Pizza", "PIZ", "Pizzas variadas", DateTime.UtcNow },
                    { Guid.NewGuid(), "Sopa", "SOP", "Sopas e caldos", DateTime.UtcNow },
                    { Guid.NewGuid(), "Vegano", "VEG", "Opções veganas", DateTime.UtcNow },
                    { Guid.NewGuid(), "Café da Manhã", "CAF", "Itens para café da manhã", DateTime.UtcNow }
                });

            // Seeding Product
            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "TypeId", "Name", "Description", "Price", "StockQuantity", "Status", "CreatedAt" },
                values: new object[,]
                {
                    { Guid.NewGuid(), lancheId, "X-Burger", "Hambúrguer simples com queijo", 15.99m, 100, 1, DateTime.UtcNow },
                    { Guid.NewGuid(), bebidaId, "Coca-Cola 350ml", "Refrigerante gelado", 6.50m, 200, 1, DateTime.UtcNow },
                    { Guid.NewGuid(), sobremesaId, "Pudim", "Pudim de leite condensado", 8.90m, 50, 1, DateTime.UtcNow },
                    { Guid.NewGuid(), pratoPrincipalId, "Feijoada", "Feijoada completa com arroz e couve", 28.00m, 30, 1, DateTime.UtcNow },
                    { Guid.NewGuid(), acompanhamentoId, "Batata Frita", "Porção média de batata frita", 9.90m, 75, 1, DateTime.UtcNow }
                });

            // Seeding User
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "TypeRole", "Name", "Email", "Password", "CreatedAt" },
                values: new object[,]
                {
                    { Guid.NewGuid(), 2, "Cidicley do Sacramento Rodrigues", "cidicley@hotmail.com", "123456", DateTime.UtcNow },
                    { Guid.NewGuid(), 2, "Gabriel Domingos", "gabriel.domingos@live.com", "123456", DateTime.UtcNow },
                    { Guid.NewGuid(), 2, "Rodrigo Grillo Perche Mahlow", "rmahlow@gmail.com", "123456", DateTime.UtcNow },
                    { Guid.NewGuid(), 2, "Marcelo Henrique Pirozzi Affonso Cedro", "marcelocedro@gmail.com", "123456", DateTime.UtcNow },
                    { Guid.NewGuid(), 2, "Gabriel Loureiro Gonçalves", "gabrielloureiro@outlook.com.br", "123456", DateTime.UtcNow }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
