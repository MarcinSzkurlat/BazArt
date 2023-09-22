using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_constant_data_to_Categories_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Painting is a visual art form where pigments or colors are applied to a surface, typically a canvas or paper, using various techniques to create images, convey emotions, or express ideas.", "https://upload.wikimedia.org/wikipedia/commons/e/e9/Soleil_levant_Claude_Monet.jpg", "Painting" },
                    { 2, "Sculpture is a three-dimensional art form in which artists manipulate materials like stone, wood, metal, or clay to create physical, often sculptural, representations of objects, people, or ideas.", "https://upload.wikimedia.org/wikipedia/commons/b/bf/6829_-_Claudio_%28Museo_Pio-Clementino%29_-_Foto_Giovanni_Dall%27Orto%2C_10_june_2011.jpg", "Sculpture" },
                    { 3, "Photography is the art and technology of capturing and recording images using light-sensitive materials or digital sensors, allowing for the creation of still or moving visual representations of scenes, subjects, and moments.", "https://upload.wikimedia.org/wikipedia/commons/e/e4/Stourhead_garden.jpg", "Photography" },
                    { 4, "Handmade art refers to creative works produced by skilled individuals using manual techniques and traditional craftsmanship, typically without the aid of automated or mass-production processes. It emphasizes the personal touch and unique qualities of each piece.", "https://upload.wikimedia.org/wikipedia/commons/0/07/Fish1.jpg", "HandMade" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
