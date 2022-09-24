using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentingSystem.Web.Migrations
{
    public partial class ModifyDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "HomePage",
                table: "Comments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomePage",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Comments",
                newName: "Content");
        }
    }
}
