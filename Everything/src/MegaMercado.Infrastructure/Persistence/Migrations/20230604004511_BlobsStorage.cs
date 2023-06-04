using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegaMercado.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BlobsStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    BlobData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blobs");
        }
    }
}
