using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5450), new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5438), "", 7, "Villa prueba", 6, 5.0 },
                    { 2, "", "Detalle de la villa...", new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5454), new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5453), "", 7, "Birlstone", 6, 5.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
