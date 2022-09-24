using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentingSystem.Web.Migrations
{
    public partial class Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Comments");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Comments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
