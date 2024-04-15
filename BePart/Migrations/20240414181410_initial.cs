using Microsoft.EntityFrameworkCore.Migrations;

namespace BePart.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<double>(type: "float", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cat", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cat",
                columns: new[] { "Id", "Age", "CatName", "Color", "Summary" },
                values: new object[,]
                {
                    { 1, 3.5, "Shadow", "Black", "A stealthy little shadow that loves to sneak around." },
                    { 2, 2.0, "Snowball", "White", "Fluffy and white as the fresh winter snow." },
                    { 3, 4.0, "Tiger", "Ginger", "Loves to play and chase lasers." },
                    { 4, 1.5, "Smoky", "Grey", "A curious explorer with a love for high places." },
                    { 5, 7.0, "Patch", "Calico", "A wise old cat with a patchy coat and gentle eyes." },
                    { 6, 0.80000000000000004, "Biscuit", "Tabby", "A playful kitten with endless energy and a penchant for mischief." },
                    { 7, 3.0, "Luna", "Siamese", "Elegant and poised, with a mysterious aura and a melodious voice." },
                    { 8, 2.5, "Oreo", "Black and White", "Sweet as a cookie, with a personality that's just as delightful." },
                    { 9, 4.5, "Simba", "Orange", "Has the heart of a lion and enjoys basking in the sun." },
                    { 10, 6.0, "Sky", "Blue", "A serene and peaceful presence, with a coat as soft as clouds." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cat");
        }
    }
}
