using Microsoft.EntityFrameworkCore.Migrations;

namespace CarForum.Migrations
{
    public partial class _initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopicFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionShort = table.Column<string>(nullable: true),
                    QuestionExtension = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Responce",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reply = table.Column<string>(nullable: true),
                    TopicFieldID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responce", x => x.id);
                    table.ForeignKey(
                        name: "FK_Responce_TopicFields_TopicFieldID",
                        column: x => x.TopicFieldID,
                        principalTable: "TopicFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Responce_TopicFieldID",
                table: "Responce",
                column: "TopicFieldID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Responce");

            migrationBuilder.DropTable(
                name: "TopicFields");
        }
    }
}
