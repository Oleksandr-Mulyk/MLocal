using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Local.Web.Migrations
{
    /// <inheritdoc />
    public partial class ToDoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ToDoItems_ToDoItemId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ToDoItems_ToDoItemId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ToDoItemId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ToDoItemId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ToDoItemId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ToDoItemId1",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserToDoItem",
                columns: table => new
                {
                    AssignedToId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToDoItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToDoItem", x => new { x.AssignedToId, x.ToDoItemId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToDoItem_AspNetUsers_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserToDoItem_ToDoItems_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserToDoItem1",
                columns: table => new
                {
                    ToDoItem1Id = table.Column<int>(type: "int", nullable: false),
                    VisibleForId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserToDoItem1", x => new { x.ToDoItem1Id, x.VisibleForId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserToDoItem1_AspNetUsers_VisibleForId",
                        column: x => x.VisibleForId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserToDoItem1_ToDoItems_ToDoItem1Id",
                        column: x => x.ToDoItem1Id,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserToDoItem_ToDoItemId",
                table: "ApplicationUserToDoItem",
                column: "ToDoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserToDoItem1_VisibleForId",
                table: "ApplicationUserToDoItem1",
                column: "VisibleForId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserToDoItem");

            migrationBuilder.DropTable(
                name: "ApplicationUserToDoItem1");

            migrationBuilder.AddColumn<int>(
                name: "ToDoItemId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToDoItemId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ToDoItemId",
                table: "AspNetUsers",
                column: "ToDoItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ToDoItemId1",
                table: "AspNetUsers",
                column: "ToDoItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ToDoItems_ToDoItemId",
                table: "AspNetUsers",
                column: "ToDoItemId",
                principalTable: "ToDoItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ToDoItems_ToDoItemId1",
                table: "AspNetUsers",
                column: "ToDoItemId1",
                principalTable: "ToDoItems",
                principalColumn: "Id");
        }
    }
}
