using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class taskpublicid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                schema: "tracker",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PublicAssigneeId",
                schema: "tracker",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PublicAuthorId",
                schema: "tracker",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PublicId",
                schema: "tracker",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                schema: "tracker",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicAssigneeId",
                schema: "tracker",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PublicAuthorId",
                schema: "tracker",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PublicId",
                schema: "tracker",
                table: "Tasks");
        }
    }
}
