using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWorkspaceAccesKeyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessKeys_Workspaces_WorkspaceId",
                table: "AccessKeys");

            migrationBuilder.DropIndex(
                name: "IX_AccessKeys_WorkspaceId",
                table: "AccessKeys");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "AccessKeys");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "AccessKeys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AccessKeys_WorkspaceId",
                table: "AccessKeys",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessKeys_Workspaces_WorkspaceId",
                table: "AccessKeys",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
