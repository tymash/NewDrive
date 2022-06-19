using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileStorage.DAL.Migrations
{
    public partial class AddedUserProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folder_AspNetUsers_UserId",
                table: "Folder");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_Folder_ParentFolderId",
                table: "StorageItems");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folder",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "StorageItems");

            migrationBuilder.RenameTable(
                name: "Folder",
                newName: "Folders");

            migrationBuilder.RenameColumn(
                name: "TrustedName",
                table: "StorageItems",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Folder_UserId",
                table: "Folders",
                newName: "IX_Folders_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Folders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Folders",
                table: "Folders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_UserId",
                table: "Folders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Folders_ParentFolderId",
                table: "StorageItems",
                column: "ParentFolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_UserId",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageItems_Folders_ParentFolderId",
                table: "StorageItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folders",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Folders");

            migrationBuilder.RenameTable(
                name: "Folders",
                newName: "Folder");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "StorageItems",
                newName: "TrustedName");

            migrationBuilder.RenameIndex(
                name: "IX_Folders_UserId",
                table: "Folder",
                newName: "IX_Folder_UserId");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "StorageItems",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Folder",
                table: "Folder",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserId",
                table: "UserAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_AspNetUsers_UserId",
                table: "Folder",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageItems_Folder_ParentFolderId",
                table: "StorageItems",
                column: "ParentFolderId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
