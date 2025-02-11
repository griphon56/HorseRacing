using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HorseRacing.Migrations.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ChangedUserId",
                        column: x => x.ChangedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<int>(type: "int", nullable: true),
                    EventTitle = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitiatorInfoText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Subsystem = table.Column<int>(type: "int", nullable: true),
                    TechProcess = table.Column<int>(type: "int", nullable: true),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventLog_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ChangedUserId", "CreatedUserId", "DateChanged", "DateCreated", "Email", "FirstName", "IsRemoved", "LastName", "Password", "Phone", "UserName" },
                values: new object[] { new Guid("8223998a-318f-460c-9464-1164ee56cb46"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "admin@race.ru", "Администратор", false, "Системы", "2A9BEAAD6666982A470776F098D3D9B6C41D1AB8D3286E7DC710DC6CE7393DB15BFDF9D0CE2C153E39A64D4FD9F04DC68FE5345BFCAA4ACB3F03", "79001112233", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_UserId",
                table: "EventLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChangedUserId",
                table: "Users",
                column: "ChangedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedUserId",
                table: "Users",
                column: "CreatedUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLog");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
