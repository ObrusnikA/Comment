using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentingSystem.Web.Migrations
{
    public partial class FileNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Comments");
        }
    }
}
