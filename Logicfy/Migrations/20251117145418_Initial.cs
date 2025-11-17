using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logicfy.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToplamXp = table.Column<int>(type: "int", nullable: false),
                    Seri = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramlamaDilleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IkonUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramlamaDilleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciGunlukSerileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    SeriSayisi = table.Column<int>(type: "int", nullable: false),
                    SonGiris = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciGunlukSerileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciGunlukSerileri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciXpLoglari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    Kaynak = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Xp = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciXpLoglari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciXpLoglari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uniteler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramlamaDiliId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    DersSayisiCache = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uniteler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uniteler_ProgramlamaDilleri_ProgramlamaDiliId",
                        column: x => x.ProgramlamaDiliId,
                        principalTable: "ProgramlamaDilleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kismlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniteId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    DersSayisiCache = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kismlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kismlar_Uniteler_UniteId",
                        column: x => x.UniteId,
                        principalTable: "Uniteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciUnitProgressleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    UniteId = table.Column<int>(type: "int", nullable: false),
                    TamamlananDersSayisi = table.Column<int>(type: "int", nullable: false),
                    ToplamDersSayisi = table.Column<int>(type: "int", nullable: false),
                    IlerlemeOrani = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciUnitProgressleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciUnitProgressleri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciUnitProgressleri_Uniteler_UniteId",
                        column: x => x.UniteId,
                        principalTable: "Uniteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniteTakipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniteId = table.Column<int>(type: "int", nullable: false),
                    TakipEdenKullaniciSayisi = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniteTakipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UniteTakipleri_Uniteler_UniteId",
                        column: x => x.UniteId,
                        principalTable: "Uniteler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dersler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisimId = table.Column<int>(type: "int", nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    TahminiSure = table.Column<int>(type: "int", nullable: false),
                    SoruSayisiCache = table.Column<int>(type: "int", nullable: false),
                    ZorlukSeviyesi = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dersler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dersler_Kismlar_KisimId",
                        column: x => x.KisimId,
                        principalTable: "Kismlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciKisimProgressleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    KisimId = table.Column<int>(type: "int", nullable: false),
                    TamamlananDersSayisi = table.Column<int>(type: "int", nullable: false),
                    ToplamDersSayisi = table.Column<int>(type: "int", nullable: false),
                    IlerlemeOrani = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciKisimProgressleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciKisimProgressleri_Kismlar_KisimId",
                        column: x => x.KisimId,
                        principalTable: "Kismlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciKisimProgressleri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DersAnalitigi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    OrtalamaTamamlamaSuresi = table.Column<double>(type: "float", nullable: false),
                    OrtalamaDogruOrani = table.Column<double>(type: "float", nullable: false),
                    EnZorSoruId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersAnalitigi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DersAnalitigi_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DersTakipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    TakipEdenKullaniciSayisi = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DersTakipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DersTakipleri_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciDersIlerlemeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    TamamlananSoruSayisi = table.Column<int>(type: "int", nullable: false),
                    ToplamSoruSayisi = table.Column<int>(type: "int", nullable: false),
                    IlerlemeOrani = table.Column<int>(type: "int", nullable: false),
                    TamamlandiMi = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciDersIlerlemeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciDersIlerlemeleri_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciDersIlerlemeleri_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciDersKayitlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciDersKayitlari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciDersKayitlari_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KullaniciDersKayitlari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KullaniciSoruCevaplari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    DogruMu = table.Column<bool>(type: "bit", nullable: false),
                    CevapJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SureMs = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KullaniciSoruCevaplari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KullaniciSoruCevaplari_Kullanicilar_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoruAnalitigi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    CevaplanmaSayisi = table.Column<int>(type: "int", nullable: false),
                    DogruSayisi = table.Column<int>(type: "int", nullable: false),
                    YanlisSayisi = table.Column<int>(type: "int", nullable: false),
                    OrtalamaSure = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruAnalitigi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoruCanliPreviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    DogruHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DogruCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GerekenEtiketlerJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GerekenStillerJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruCanliPreviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoruFonksiyonCozumleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    CozumKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruFonksiyonCozumleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoruKelimeBloklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    DogruKod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KelimelerJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruKelimeBloklari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(type: "int", nullable: false),
                    SoruTipi = table.Column<int>(type: "int", nullable: false),
                    SoruMetni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KodMetni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seviye = table.Column<int>(type: "int", nullable: false),
                    DogruCevapId = table.Column<int>(type: "int", nullable: true),
                    EkVeriJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorular_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoruSecenekleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruId = table.Column<int>(type: "int", nullable: false),
                    SecenekMetni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruSecenekleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoruSecenekleri_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalTable: "Sorular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DersAnalitigi_DersId",
                table: "DersAnalitigi",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_Dersler_KisimId",
                table: "Dersler",
                column: "KisimId");

            migrationBuilder.CreateIndex(
                name: "IX_DersTakipleri_DersId",
                table: "DersTakipleri",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_Kismlar_UniteId",
                table: "Kismlar",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciDersIlerlemeleri_DersId",
                table: "KullaniciDersIlerlemeleri",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciDersIlerlemeleri_KullaniciId",
                table: "KullaniciDersIlerlemeleri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciDersKayitlari_DersId",
                table: "KullaniciDersKayitlari",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciDersKayitlari_KullaniciId",
                table: "KullaniciDersKayitlari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciGunlukSerileri_KullaniciId",
                table: "KullaniciGunlukSerileri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciKisimProgressleri_KisimId",
                table: "KullaniciKisimProgressleri",
                column: "KisimId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciKisimProgressleri_KullaniciId",
                table: "KullaniciKisimProgressleri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciSoruCevaplari_KullaniciId",
                table: "KullaniciSoruCevaplari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciSoruCevaplari_SoruId",
                table: "KullaniciSoruCevaplari",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciUnitProgressleri_KullaniciId",
                table: "KullaniciUnitProgressleri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciUnitProgressleri_UniteId",
                table: "KullaniciUnitProgressleri",
                column: "UniteId");

            migrationBuilder.CreateIndex(
                name: "IX_KullaniciXpLoglari_KullaniciId",
                table: "KullaniciXpLoglari",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_SoruAnalitigi_SoruId",
                table: "SoruAnalitigi",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_SoruCanliPreviews_SoruId",
                table: "SoruCanliPreviews",
                column: "SoruId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SoruFonksiyonCozumleri_SoruId",
                table: "SoruFonksiyonCozumleri",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_SoruKelimeBloklari_SoruId",
                table: "SoruKelimeBloklari",
                column: "SoruId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_DersId",
                table: "Sorular",
                column: "DersId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_DogruCevapId",
                table: "Sorular",
                column: "DogruCevapId");

            migrationBuilder.CreateIndex(
                name: "IX_SoruSecenekleri_SoruId",
                table: "SoruSecenekleri",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Uniteler_ProgramlamaDiliId",
                table: "Uniteler",
                column: "ProgramlamaDiliId");

            migrationBuilder.CreateIndex(
                name: "IX_UniteTakipleri_UniteId",
                table: "UniteTakipleri",
                column: "UniteId");

            migrationBuilder.AddForeignKey(
                name: "FK_KullaniciSoruCevaplari_Sorular_SoruId",
                table: "KullaniciSoruCevaplari",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoruAnalitigi_Sorular_SoruId",
                table: "SoruAnalitigi",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoruCanliPreviews_Sorular_SoruId",
                table: "SoruCanliPreviews",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoruFonksiyonCozumleri_Sorular_SoruId",
                table: "SoruFonksiyonCozumleri",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoruKelimeBloklari_Sorular_SoruId",
                table: "SoruKelimeBloklari",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sorular_SoruSecenekleri_DogruCevapId",
                table: "Sorular",
                column: "DogruCevapId",
                principalTable: "SoruSecenekleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sorular_Dersler_DersId",
                table: "Sorular");

            migrationBuilder.DropForeignKey(
                name: "FK_SoruSecenekleri_Sorular_SoruId",
                table: "SoruSecenekleri");

            migrationBuilder.DropTable(
                name: "DersAnalitigi");

            migrationBuilder.DropTable(
                name: "DersTakipleri");

            migrationBuilder.DropTable(
                name: "KullaniciDersIlerlemeleri");

            migrationBuilder.DropTable(
                name: "KullaniciDersKayitlari");

            migrationBuilder.DropTable(
                name: "KullaniciGunlukSerileri");

            migrationBuilder.DropTable(
                name: "KullaniciKisimProgressleri");

            migrationBuilder.DropTable(
                name: "KullaniciSoruCevaplari");

            migrationBuilder.DropTable(
                name: "KullaniciUnitProgressleri");

            migrationBuilder.DropTable(
                name: "KullaniciXpLoglari");

            migrationBuilder.DropTable(
                name: "SoruAnalitigi");

            migrationBuilder.DropTable(
                name: "SoruCanliPreviews");

            migrationBuilder.DropTable(
                name: "SoruFonksiyonCozumleri");

            migrationBuilder.DropTable(
                name: "SoruKelimeBloklari");

            migrationBuilder.DropTable(
                name: "UniteTakipleri");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Dersler");

            migrationBuilder.DropTable(
                name: "Kismlar");

            migrationBuilder.DropTable(
                name: "Uniteler");

            migrationBuilder.DropTable(
                name: "ProgramlamaDilleri");

            migrationBuilder.DropTable(
                name: "Sorular");

            migrationBuilder.DropTable(
                name: "SoruSecenekleri");
        }
    }
}
