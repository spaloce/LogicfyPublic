using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicfy.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToplamXp",
                table: "Kullanicilar",
                newName: "XP");

            migrationBuilder.RenameColumn(
                name: "Seri",
                table: "Kullanicilar",
                newName: "Streak");

            migrationBuilder.AddColumn<int>(
                name: "SoruId1",
                table: "SoruSecenekleri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "KayitTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Kullanicilar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Seviye",
                table: "Kullanicilar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SonGirisTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoruSecenekleri_SoruId1",
                table: "SoruSecenekleri",
                column: "SoruId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SoruSecenekleri_Sorular_SoruId1",
                table: "SoruSecenekleri",
                column: "SoruId1",
                principalTable: "Sorular",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoruSecenekleri_Sorular_SoruId1",
                table: "SoruSecenekleri");

            migrationBuilder.DropIndex(
                name: "IX_SoruSecenekleri_SoruId1",
                table: "SoruSecenekleri");

            migrationBuilder.DropColumn(
                name: "SoruId1",
                table: "SoruSecenekleri");

            migrationBuilder.DropColumn(
                name: "KayitTarihi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "Seviye",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SonGirisTarihi",
                table: "Kullanicilar");

            migrationBuilder.RenameColumn(
                name: "XP",
                table: "Kullanicilar",
                newName: "ToplamXp");

            migrationBuilder.RenameColumn(
                name: "Streak",
                table: "Kullanicilar",
                newName: "Seri");
        }
    }
}
