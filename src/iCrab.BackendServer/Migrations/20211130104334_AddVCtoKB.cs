using Microsoft.EntityFrameworkCore.Migrations;

namespace iCrabee.BackendServer.Migrations
{
    public partial class AddVCtoKB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "KnowledgeBases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "KnowledgeBases");
        }
    }
}
