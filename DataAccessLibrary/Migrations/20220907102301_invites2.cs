using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    public partial class invites2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TodoLists_TodoListId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TodoListId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TodoListId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Collabs",
                columns: table => new
                {
                    CollabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TodoListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collabs", x => x.CollabId);
                    table.ForeignKey(
                        name: "FK_Collabs_TodoLists_TodoListId",
                        column: x => x.TodoListId,
                        principalTable: "TodoLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collabs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collabs_TodoListId",
                table: "Collabs",
                column: "TodoListId");

            migrationBuilder.CreateIndex(
                name: "IX_Collabs_UserId",
                table: "Collabs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collabs");

            migrationBuilder.AddColumn<Guid>(
                name: "TodoListId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TodoListId",
                table: "Users",
                column: "TodoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TodoLists_TodoListId",
                table: "Users",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id");
        }
    }
}
