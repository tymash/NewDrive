using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorage.DAL.Migrations
{
    public partial class AddedDefaultDateValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 26, 18, 32, 13, 270, DateTimeKind.Local).AddTicks(3170),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Folders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 26, 18, 32, 13, 270, DateTimeKind.Local).AddTicks(4330),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "files",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 6, 26, 18, 32, 13, 270, DateTimeKind.Local).AddTicks(3170));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Folders",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 6, 26, 18, 32, 13, 270, DateTimeKind.Local).AddTicks(4330));
        }
    }
}
