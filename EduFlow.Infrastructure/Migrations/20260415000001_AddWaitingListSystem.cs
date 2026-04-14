using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWaitingListSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaitingListEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueuePosition = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingListEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitingListEntries_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaitingListEntries_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaitingListEntry_RequestTime",
                table: "WaitingListEntries",
                column: "RequestTime");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingListEntry_SessionId",
                table: "WaitingListEntries",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingListEntry_StudentId_SessionId",
                table: "WaitingListEntries",
                columns: new[] { "StudentId", "SessionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaitingListEntries");
        }
    }
}
