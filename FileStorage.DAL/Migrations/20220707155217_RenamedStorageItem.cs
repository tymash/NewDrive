using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorage.DAL.Migrations
{
    public partial class RenamedStorageItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_files_AspNetUsers_UserId",
                table: "files");

            migrationBuilder.DropForeignKey(
                name: "FK_files_Folders_ParentFolderId",
                table: "files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_files",
                table: "files");

            migrationBuilder.RenameTable(
                name: "files",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_files_UserId",
                table: "Files",
                newName: "IX_Files_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_files_ParentFolderId",
                table: "Files",
                newName: "IX_Files_ParentFolderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Folders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 7, 18, 52, 17, 96, DateTimeKind.Local).AddTicks(7300),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 6, 26, 22, 15, 12, 163, DateTimeKind.Local).AddTicks(7600));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 7, 7, 18, 52, 17, 96, DateTimeKind.Local).AddTicks(6090),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 6, 26, 22, 15, 12, 163, DateTimeKind.Local).AddTicks(6420));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Folders_ParentFolderId",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "files");

            migrationBuilder.RenameIndex(
                name: "IX_Files_UserId",
                table: "files",
                newName: "IX_files_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_ParentFolderId",
                table: "files",
                newName: "IX_files_ParentFolderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Folders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 26, 22, 15, 12, 163, DateTimeKind.Local).AddTicks(7600),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 7, 18, 52, 17, 96, DateTimeKind.Local).AddTicks(7300));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 26, 22, 15, 12, 163, DateTimeKind.Local).AddTicks(6420),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 7, 7, 18, 52, 17, 96, DateTimeKind.Local).AddTicks(6090));

            migrationBuilder.AddPrimaryKey(
                name: "PK_files",
                table: "files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_files_AspNetUsers_UserId",
                table: "files",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_files_Folders_ParentFolderId",
                table: "files",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
