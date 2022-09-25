using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentingSystem.Web.Migrations
{
    public partial class Modification2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebBrowser",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "WebBrowser",
                table: "Comments");
        }
    }
}
