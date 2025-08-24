using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    VersionRow = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
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
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    DefaultBet = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VersionRow = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ChangedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_ChangedUserId",
                        column: x => x.ChangedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameDeckCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardSuit = table.Column<int>(type: "int", nullable: false),
                    CardRank = table.Column<int>(type: "int", nullable: false),
                    CardOrder = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeckCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameDeckCards_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    CardSuit = table.Column<int>(type: "int", nullable: true),
                    CardRank = table.Column<int>(type: "int", nullable: true),
                    CardOrder = table.Column<int>(type: "int", nullable: true),
                    HorseSuit = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    Place = table.Column<int>(type: "int", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameEvents_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameHorsePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HorseSuit = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameHorsePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameHorsePositions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BetSuit = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Place = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    BetSuit = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameResults_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ChangedUserId", "CreatedUserId", "DateChanged", "DateCreated", "Email", "FirstName", "IsRemoved", "LastName", "Password", "Phone", "UserName" },
                values: new object[,]
                {
                    { new Guid("141fcb82-6639-4932-a68d-af84f09ef42d"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "petr@race.ru", "Петр", false, "Крести", "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F", "79001112222", "petr" },
                    { new Guid("17b6f19e-56b1-485c-a16a-c60cec5cdaa6"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "alex@race.ru", "Алексей", false, "Бубна", "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F", "79001112244", "alex" },
                    { new Guid("8223998a-318f-460c-9464-1164ee56cb46"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "admin@race.ru", "Администратор", false, "Системы", "82E3B4B3D57F6D4112A02310EA2E8F9517BC19BD6EBF1EC95E0C7AA961B3B3F2AE24BB2CA278CB190D24B55241AC893C9E590717106F3FB18070", "79001112233", "admin" },
                    { new Guid("8f2facbc-2ef4-4fe1-b9e0-e3f877edb3c3"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "ivan@race.ru", "Иван", false, "Пика", "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F", "79001112211", "ivan" },
                    { new Guid("a1f52f7e-6dd6-4ff9-bded-78305b42c81e"), null, null, null, new DateTime(2024, 12, 31, 21, 0, 0, 0, DateTimeKind.Utc), "john@race.ru", "Джонни", false, "Черва", "A44DC50E7693F034C1C6032F04E7E5152729DF8891D15DD1F485F7C98FE62AA0113FCB77745A024BF3573AD54837D6D8D0DDD61E27E06B1DC18F", "79001112233", "john" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "UserId" },
                values: new object[,]
                {
                    { new Guid("1223998a-318f-460c-9464-1164ee56cb46"), 0m, new Guid("8223998a-318f-460c-9464-1164ee56cb46") },
                    { new Guid("12e3693b-c52c-418b-8053-7f1d2c173f73"), 100m, new Guid("141fcb82-6639-4932-a68d-af84f09ef42d") },
                    { new Guid("6e61dbec-d4c9-4f17-ba0d-2920d714e3d8"), 100m, new Guid("8f2facbc-2ef4-4fe1-b9e0-e3f877edb3c3") },
                    { new Guid("7435e47c-7336-4c28-badc-0fa526756616"), 100m, new Guid("17b6f19e-56b1-485c-a16a-c60cec5cdaa6") },
                    { new Guid("b43ab810-b393-454d-b35f-15975d20e33b"), 100m, new Guid("a1f52f7e-6dd6-4ff9-bded-78305b42c81e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_UserId",
                table: "EventLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDeckCards_GameId",
                table: "GameDeckCards",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameEvents_GameId",
                table: "GameEvents",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameHorsePositions_GameId",
                table: "GameHorsePositions",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_GameId",
                table: "GamePlayers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_UserId",
                table: "GamePlayers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_GameId",
                table: "GameResults",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameResults_UserId",
                table: "GameResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_ChangedUserId",
                table: "Games",
                column: "ChangedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CreatedUserId",
                table: "Games",
                column: "CreatedUserId");

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
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "EventLog");

            migrationBuilder.DropTable(
                name: "GameDeckCards");

            migrationBuilder.DropTable(
                name: "GameEvents");

            migrationBuilder.DropTable(
                name: "GameHorsePositions");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "GameResults");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
