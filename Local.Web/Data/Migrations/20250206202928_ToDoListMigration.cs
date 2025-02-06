using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Local.Web.Migrations
{
    /// <inheritdoc />
    public partial class ToDoListMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeathLine = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusChanged = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusChangedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusComment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoItems_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ToDoItems_AspNetUsers_StatusChangedById",
                        column: x => x.StatusChangedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_CreatedById",
                table: "ToDoItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_StatusChangedById",
                table: "ToDoItems",
                column: "StatusChangedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserToDoItem");

            migrationBuilder.DropTable(
                name: "ApplicationUserToDoItem1");

            migrationBuilder.DropTable(
                name: "ToDoItems");
        }
    }
}
