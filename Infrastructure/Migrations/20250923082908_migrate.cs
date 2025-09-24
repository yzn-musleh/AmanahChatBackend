using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashedApiKey",
                table: "AccessKeys",
                newName: "KeyId");

            migrationBuilder.AddColumn<string>(
                name: "KeyHash",
                table: "AccessKeys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "AccessKeys",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyHash",
                table: "AccessKeys");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "AccessKeys");

            migrationBuilder.RenameColumn(
                name: "KeyId",
                table: "AccessKeys",
                newName: "HashedApiKey");
        }
    }
}
