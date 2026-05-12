using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Minemation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalSyncFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ekipman",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ekipmanAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ekipmanMarka = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ekipmanModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sonBakimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    gelecekBakimTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    seriNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RFIDEtiket = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ureticiFirma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tedarikciFirma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uretimYili = table.Column<DateTime>(type: "datetime2", nullable: false),
                    boyut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    agirlik = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    satinAlmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    garantiBaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    teknikDokuman = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    kullanimKilavuzu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    garantiBelgesi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    bakimFormu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    satinAlmaBelgesi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    operasyonTuru = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ekipman", x => x.ekipmanId);
                });

            migrationBuilder.CreateTable(
                name: "Personeller",
                columns: table => new
                {
                    personelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    personelAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    personelSoyadi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    uzmanlik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tckn = table.Column<string>(type: "char(11)", nullable: false),
                    personelDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dogumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ikinciTelNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    eposta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iseGirisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    calisanTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rfidKartNumarasi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    kullaniciRolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departman = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    calismaKonumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sifreHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sonGirisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personeller", x => x.personelId);
                });

            migrationBuilder.CreateTable(
                name: "Ekskavator",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    plaka = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    kovaKapasitesi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    motorGucu = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    maksimumKaziDerinligi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paletTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bomUzunlugu = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ekskavator", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_Ekskavator_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ElAletleri",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    gucKaynagiTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bataryaKapasitesi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    kullanimAmaci = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElAletleri", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_ElAletleri_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hafriyat",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    plaka = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    damperHacmi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    azamiYukAgirligi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    dingilSayisi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hafriyat", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_Hafriyat_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kepce",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    plaka = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    yuklemeKapasitesi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    kovaKapasitesi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    bosaltmaYuksekligi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    devrilmeYuku = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kepce", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_Kepce_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kirici",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    darbeEnerjisi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    dakikadakiDarbeSayisi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    calismaBasinci = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ucTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    gerekenYagDebisi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kirici", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_Kirici_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensor",
                columns: table => new
                {
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    sensorTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sensorDurumu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    minEsikDeger = table.Column<double>(type: "float", nullable: false),
                    maxEsikDeger = table.Column<double>(type: "float", nullable: false),
                    hassasiyet = table.Column<double>(type: "float", nullable: false),
                    baglantiProtokolu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    haberlesmeTipi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensor", x => x.ekipmanId);
                    table.ForeignKey(
                        name: "FK_Sensor_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcilDurumIletisim",
                columns: table => new
                {
                    acilDurumKisisiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    acilDurumKisileriAd = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    acilDurumKisileriSoyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    acilDurumKisileriYakinlik = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    acilDurumKisileriTelNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcilDurumIletisim", x => x.acilDurumKisisiId);
                    table.ForeignKey(
                        name: "FK_AcilDurumIletisim_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogKaydi",
                columns: table => new
                {
                    logKaydiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    islemTipi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    logKaydiAciklamasi = table.Column<string>(type: "text", nullable: false),
                    logKaydiTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ipAdresi = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    onemSeviyesi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    durum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: true),
                    ekipmanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogKaydi", x => x.logKaydiID);
                    table.ForeignKey(
                        name: "FK_LogKaydi_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_LogKaydi_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Rapor",
                columns: table => new
                {
                    raporId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    raporAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    raporTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    raporOlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    raporAciklamasi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    raporDosyaYolu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    zamanAraligi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: true),
                    ekipmanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rapor", x => x.raporId);
                    table.ForeignKey(
                        name: "FK_Rapor_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Rapor_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SaglikBilgileri",
                columns: table => new
                {
                    personelId = table.Column<int>(type: "int", nullable: false),
                    kanGrubu = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    saglikDurumu = table.Column<string>(type: "text", nullable: false),
                    kronikHastaliklar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alerjiler = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    saglikCalismaKisitlamalari = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acilDurumNotu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sonMuayeneTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaglikBilgileri", x => x.personelId);
                    table.ForeignKey(
                        name: "FK_SaglikBilgileri_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TakipCihazi",
                columns: table => new
                {
                    takipCihaziId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    takipCihaziSeriNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    takipCihaziTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    takipCihaziModeli = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    takipCihaziDurumu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    takipCihaziSonBaglantiZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    takipCihaziHaberlesmeProtokolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pilSeviyesi = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: true),
                    ekipmanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakipCihazi", x => x.takipCihaziId);
                    table.ForeignKey(
                        name: "FK_TakipCihazi_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TakipCihazi_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Vaka",
                columns: table => new
                {
                    vakaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vakaTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vakaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    vakaCiddiyetSeviyesi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vakaDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vakaAciklamasi = table.Column<string>(type: "text", nullable: false),
                    vakaOlusmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vakaKapanmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    olayNedeni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: true),
                    raporlayanEkipmanId = table.Column<int>(type: "int", nullable: true),
                    ilgiliEkipmanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaka", x => x.vakaId);
                    table.ForeignKey(
                        name: "FK_Vaka_Ekipman_ilgiliEkipmanId",
                        column: x => x.ilgiliEkipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vaka_Ekipman_raporlayanEkipmanId",
                        column: x => x.raporlayanEkipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vaka_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vardiya",
                columns: table => new
                {
                    vardiyaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vardiyaAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    vardiyaTanimi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vardiyaBaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vardiyaBitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vardiyaOlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    vardiyaSupervizoru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    personelSayisi = table.Column<int>(type: "int", nullable: false),
                    ekipmanSayisi = table.Column<int>(type: "int", nullable: false),
                    ekipSayisi = table.Column<int>(type: "int", nullable: false),
                    vardiyaDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vardiyaTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    toplaVardiyaSuresi = table.Column<int>(type: "int", nullable: false),
                    calismaBolgesi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    operasyonTipi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    operasyonRiskSeviyesi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vardiyaNotlari = table.Column<string>(type: "text", nullable: false),
                    ekipmanOperatoru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ekipmanId = table.Column<int>(type: "int", nullable: true),
                    vardiyaSorumlusu = table.Column<int>(type: "int", nullable: false),
                    vardiyaIsgSorumlusu = table.Column<int>(type: "int", nullable: false),
                    vardiyaTeknikSorumlusu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vardiya", x => x.vardiyaId);
                    table.ForeignKey(
                        name: "FK_Vardiya_Ekipman_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Vardiya_Personeller_vardiyaIsgSorumlusu",
                        column: x => x.vardiyaIsgSorumlusu,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vardiya_Personeller_vardiyaSorumlusu",
                        column: x => x.vardiyaSorumlusu,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vardiya_Personeller_vardiyaTeknikSorumlusu",
                        column: x => x.vardiyaTeknikSorumlusu,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EkipmanRaporu",
                columns: table => new
                {
                    raporId = table.Column<int>(type: "int", nullable: false),
                    ekipmanTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    arizaSayisi = table.Column<int>(type: "int", nullable: false),
                    calismaSuresi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EkipmanRaporu", x => x.raporId);
                    table.ForeignKey(
                        name: "FK_EkipmanRaporu_Rapor_raporId",
                        column: x => x.raporId,
                        principalTable: "Rapor",
                        principalColumn: "raporId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonelRaporu",
                columns: table => new
                {
                    raporId = table.Column<int>(type: "int", nullable: false),
                    uzmanlikAlani = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    personelSayisi = table.Column<int>(type: "int", nullable: false),
                    calismaSuresi = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonelRaporu", x => x.raporId);
                    table.ForeignKey(
                        name: "FK_PersonelRaporu_Rapor_raporId",
                        column: x => x.raporId,
                        principalTable: "Rapor",
                        principalColumn: "raporId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VakaRaporu",
                columns: table => new
                {
                    raporId = table.Column<int>(type: "int", nullable: false),
                    ciddiyetSeviyesi = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cozumSuresi = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: true),
                    raporlayanEkipmanId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VakaRaporu", x => x.raporId);
                    table.ForeignKey(
                        name: "FK_VakaRaporu_Ekipman_raporlayanEkipmanId",
                        column: x => x.raporlayanEkipmanId,
                        principalTable: "Ekipman",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VakaRaporu_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VakaRaporu_Rapor_raporId",
                        column: x => x.raporId,
                        principalTable: "Rapor",
                        principalColumn: "raporId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ekip",
                columns: table => new
                {
                    ekipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ekipUyeSayisi = table.Column<int>(type: "int", nullable: false),
                    personelGorevi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    durum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    personelId = table.Column<int>(type: "int", nullable: false),
                    vardiyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ekip", x => x.ekipId);
                    table.ForeignKey(
                        name: "FK_Ekip_Personeller_personelId",
                        column: x => x.personelId,
                        principalTable: "Personeller",
                        principalColumn: "personelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ekip_Vardiya_vardiyaId",
                        column: x => x.vardiyaId,
                        principalTable: "Vardiya",
                        principalColumn: "vardiyaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorVerisi",
                columns: table => new
                {
                    sensorVerisiId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    deger = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    birim = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    olcumTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ekipmanId = table.Column<int>(type: "int", nullable: false),
                    vardiyaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorVerisi", x => x.sensorVerisiId);
                    table.ForeignKey(
                        name: "FK_SensorVerisi_Sensor_ekipmanId",
                        column: x => x.ekipmanId,
                        principalTable: "Sensor",
                        principalColumn: "ekipmanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorVerisi_Vardiya_vardiyaId",
                        column: x => x.vardiyaId,
                        principalTable: "Vardiya",
                        principalColumn: "vardiyaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Aksiyon",
                columns: table => new
                {
                    mudahaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mudahaleBaslangicSaati = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mudahaleBitisSaati = table.Column<DateTime>(type: "datetime2", nullable: false),
                    mudahaleTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    uygulananCozum = table.Column<string>(type: "text", nullable: false),
                    ekipId = table.Column<int>(type: "int", nullable: false),
                    vakaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aksiyon", x => x.mudahaleId);
                    table.ForeignKey(
                        name: "FK_Aksiyon_Ekip_ekipId",
                        column: x => x.ekipId,
                        principalTable: "Ekip",
                        principalColumn: "ekipId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aksiyon_Vaka_vakaId",
                        column: x => x.vakaId,
                        principalTable: "Vaka",
                        principalColumn: "vakaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Ekipman",
                columns: new[] { "ekipmanId", "RFIDEtiket", "agirlik", "bakimFormu", "boyut", "durum", "ekipmanAdi", "ekipmanMarka", "ekipmanModel", "garantiBaslangicTarihi", "garantiBelgesi", "gelecekBakimTarihi", "kullanimKilavuzu", "operasyonTuru", "satinAlmaBelgesi", "satinAlmaTarihi", "seriNo", "sonBakimTarihi", "tedarikciFirma", "teknikDokuman", "ureticiFirma", "uretimYili" },
                values: new object[,]
                {
                    { 501, "RFID-EQ-501", 0m, "/docs/maint_501.pdf", 0m, "Aktif", "Kamyon TR-1", "Volvo", "FMX-460", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_501.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_501.pdf", "Nakliye", "/docs/invoice_501.pdf", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-501", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_501.pdf", "Global Mining Tech", new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 502, "RFID-EQ-502", 0m, "/docs/maint_502.pdf", 0m, "Aktif", "Ekskavatör EX-2", "Caterpillar", "320-GC", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_502.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_502.pdf", "Kazı", "/docs/invoice_502.pdf", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-502", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_502.pdf", "Global Mining Tech", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 503, "RFID-EQ-503", 0m, "/docs/maint_503.pdf", 0m, "Aktif", "Kamyon TR-3", "Volvo", "FMX-460", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_503.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_503.pdf", "Nakliye", "/docs/invoice_503.pdf", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-503", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_503.pdf", "Global Mining Tech", new DateTime(2022, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 504, "RFID-EQ-504", 0m, "/docs/maint_504.pdf", 0m, "Aktif", "Ekskavatör EX-4", "Caterpillar", "320-GC", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_504.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_504.pdf", "Kazı", "/docs/invoice_504.pdf", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-504", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_504.pdf", "Global Mining Tech", new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 505, "RFID-EQ-505", 0m, "/docs/maint_505.pdf", 0m, "Aktif", "Kamyon TR-5", "Volvo", "FMX-460", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_505.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_505.pdf", "Nakliye", "/docs/invoice_505.pdf", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-505", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_505.pdf", "Global Mining Tech", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 506, "RFID-EQ-506", 0m, "/docs/maint_506.pdf", 0m, "Aktif", "Ekskavatör EX-6", "Caterpillar", "320-GC", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_506.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_506.pdf", "Kazı", "/docs/invoice_506.pdf", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-506", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_506.pdf", "Global Mining Tech", new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 507, "RFID-EQ-507", 0m, "/docs/maint_507.pdf", 0m, "Aktif", "Kamyon TR-7", "Volvo", "FMX-460", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_507.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_507.pdf", "Nakliye", "/docs/invoice_507.pdf", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-507", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_507.pdf", "Global Mining Tech", new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 508, "RFID-EQ-508", 0m, "/docs/maint_508.pdf", 0m, "Aktif", "Ekskavatör EX-8", "Caterpillar", "320-GC", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_508.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_508.pdf", "Kazı", "/docs/invoice_508.pdf", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-508", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_508.pdf", "Global Mining Tech", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 509, "RFID-EQ-509", 0m, "/docs/maint_509.pdf", 0m, "Aktif", "Kamyon TR-9", "Volvo", "FMX-460", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_509.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_509.pdf", "Nakliye", "/docs/invoice_509.pdf", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-509", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_509.pdf", "Global Mining Tech", new DateTime(2022, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 510, "RFID-EQ-510", 0m, "/docs/maint_510.pdf", 0m, "Aktif", "Ekskavatör EX-10", "Caterpillar", "320-GC", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_510.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_510.pdf", "Kazı", "/docs/invoice_510.pdf", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-510", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_510.pdf", "Global Mining Tech", new DateTime(2022, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 511, "RFID-EQ-511", 0m, "/docs/maint_511.pdf", 0m, "Aktif", "Kamyon TR-11", "Volvo", "FMX-460", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_511.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_511.pdf", "Nakliye", "/docs/invoice_511.pdf", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-511", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_511.pdf", "Global Mining Tech", new DateTime(2022, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 512, "RFID-EQ-512", 0m, "/docs/maint_512.pdf", 0m, "Aktif", "Ekskavatör EX-12", "Caterpillar", "320-GC", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_512.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_512.pdf", "Kazı", "/docs/invoice_512.pdf", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-512", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_512.pdf", "Global Mining Tech", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 513, "RFID-EQ-513", 0m, "/docs/maint_513.pdf", 0m, "Aktif", "Kamyon TR-13", "Volvo", "FMX-460", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_513.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_513.pdf", "Nakliye", "/docs/invoice_513.pdf", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-513", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_513.pdf", "Global Mining Tech", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 514, "RFID-EQ-514", 0m, "/docs/maint_514.pdf", 0m, "Aktif", "Ekskavatör EX-14", "Caterpillar", "320-GC", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_514.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_514.pdf", "Kazı", "/docs/invoice_514.pdf", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-514", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_514.pdf", "Global Mining Tech", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 515, "RFID-EQ-515", 0m, "/docs/maint_515.pdf", 0m, "Aktif", "Kamyon TR-15", "Volvo", "FMX-460", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_515.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_515.pdf", "Nakliye", "/docs/invoice_515.pdf", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-515", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_515.pdf", "Global Mining Tech", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 516, "RFID-EQ-516", 0m, "/docs/maint_516.pdf", 0m, "Aktif", "Ekskavatör EX-16", "Caterpillar", "320-GC", new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_516.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_516.pdf", "Kazı", "/docs/invoice_516.pdf", new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-516", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_516.pdf", "Global Mining Tech", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 517, "RFID-EQ-517", 0m, "/docs/maint_517.pdf", 0m, "Aktif", "Kamyon TR-17", "Volvo", "FMX-460", new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_517.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_517.pdf", "Nakliye", "/docs/invoice_517.pdf", new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-517", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_517.pdf", "Global Mining Tech", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 518, "RFID-EQ-518", 0m, "/docs/maint_518.pdf", 0m, "Aktif", "Ekskavatör EX-18", "Caterpillar", "320-GC", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_518.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_518.pdf", "Kazı", "/docs/invoice_518.pdf", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-518", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_518.pdf", "Global Mining Tech", new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 519, "RFID-EQ-519", 0m, "/docs/maint_519.pdf", 0m, "Aktif", "Kamyon TR-19", "Volvo", "FMX-460", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_519.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_519.pdf", "Nakliye", "/docs/invoice_519.pdf", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-519", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_519.pdf", "Global Mining Tech", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 520, "RFID-EQ-520", 0m, "/docs/maint_520.pdf", 0m, "Aktif", "Ekskavatör EX-20", "Caterpillar", "320-GC", new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_520.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_520.pdf", "Kazı", "/docs/invoice_520.pdf", new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-520", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_520.pdf", "Global Mining Tech", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 521, "RFID-EQ-521", 0m, "/docs/maint_521.pdf", 0m, "Aktif", "Kamyon TR-21", "Volvo", "FMX-460", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_521.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_521.pdf", "Nakliye", "/docs/invoice_521.pdf", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-521", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_521.pdf", "Global Mining Tech", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 522, "RFID-EQ-522", 0m, "/docs/maint_522.pdf", 0m, "Aktif", "Ekskavatör EX-22", "Caterpillar", "320-GC", new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_522.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_522.pdf", "Kazı", "/docs/invoice_522.pdf", new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-522", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_522.pdf", "Global Mining Tech", new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 523, "RFID-EQ-523", 0m, "/docs/maint_523.pdf", 0m, "Aktif", "Kamyon TR-23", "Volvo", "FMX-460", new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_523.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_523.pdf", "Nakliye", "/docs/invoice_523.pdf", new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-523", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_523.pdf", "Global Mining Tech", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 524, "RFID-EQ-524", 0m, "/docs/maint_524.pdf", 0m, "Aktif", "Ekskavatör EX-24", "Caterpillar", "320-GC", new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_524.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_524.pdf", "Kazı", "/docs/invoice_524.pdf", new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-524", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_524.pdf", "Global Mining Tech", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 525, "RFID-EQ-525", 0m, "/docs/maint_525.pdf", 0m, "Aktif", "Kamyon TR-25", "Volvo", "FMX-460", new DateTime(2024, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_525.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_525.pdf", "Nakliye", "/docs/invoice_525.pdf", new DateTime(2024, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-525", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_525.pdf", "Global Mining Tech", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 526, "RFID-EQ-526", 0m, "/docs/maint_526.pdf", 0m, "Aktif", "Ekskavatör EX-26", "Caterpillar", "320-GC", new DateTime(2024, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_526.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_526.pdf", "Kazı", "/docs/invoice_526.pdf", new DateTime(2024, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-526", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_526.pdf", "Global Mining Tech", new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 527, "RFID-EQ-527", 0m, "/docs/maint_527.pdf", 0m, "Aktif", "Kamyon TR-27", "Volvo", "FMX-460", new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_527.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_527.pdf", "Nakliye", "/docs/invoice_527.pdf", new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-527", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_527.pdf", "Global Mining Tech", new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 528, "RFID-EQ-528", 0m, "/docs/maint_528.pdf", 0m, "Aktif", "Ekskavatör EX-28", "Caterpillar", "320-GC", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_528.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_528.pdf", "Kazı", "/docs/invoice_528.pdf", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-528", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_528.pdf", "Global Mining Tech", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 529, "RFID-EQ-529", 0m, "/docs/maint_529.pdf", 0m, "Aktif", "Kamyon TR-29", "Volvo", "FMX-460", new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_529.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_529.pdf", "Nakliye", "/docs/invoice_529.pdf", new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-529", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_529.pdf", "Global Mining Tech", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 530, "RFID-EQ-530", 0m, "/docs/maint_530.pdf", 0m, "Aktif", "Ekskavatör EX-30", "Caterpillar", "320-GC", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_530.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_530.pdf", "Kazı", "/docs/invoice_530.pdf", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-530", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_530.pdf", "Global Mining Tech", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 531, "RFID-EQ-531", 0m, "/docs/maint_531.pdf", 0m, "Aktif", "Kamyon TR-31", "Volvo", "FMX-460", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_531.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_531.pdf", "Nakliye", "/docs/invoice_531.pdf", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-531", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_531.pdf", "Global Mining Tech", new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 532, "RFID-EQ-532", 0m, "/docs/maint_532.pdf", 0m, "Aktif", "Ekskavatör EX-32", "Caterpillar", "320-GC", new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_532.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_532.pdf", "Kazı", "/docs/invoice_532.pdf", new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-532", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_532.pdf", "Global Mining Tech", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 533, "RFID-EQ-533", 0m, "/docs/maint_533.pdf", 0m, "Aktif", "Kamyon TR-33", "Volvo", "FMX-460", new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_533.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_533.pdf", "Nakliye", "/docs/invoice_533.pdf", new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-533", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_533.pdf", "Global Mining Tech", new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 534, "RFID-EQ-534", 0m, "/docs/maint_534.pdf", 0m, "Aktif", "Ekskavatör EX-34", "Caterpillar", "320-GC", new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_534.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_534.pdf", "Kazı", "/docs/invoice_534.pdf", new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-534", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_534.pdf", "Global Mining Tech", new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 535, "RFID-EQ-535", 0m, "/docs/maint_535.pdf", 0m, "Aktif", "Kamyon TR-35", "Volvo", "FMX-460", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_535.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_535.pdf", "Nakliye", "/docs/invoice_535.pdf", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-535", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_535.pdf", "Global Mining Tech", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 536, "RFID-EQ-536", 0m, "/docs/maint_536.pdf", 0m, "Aktif", "Ekskavatör EX-36", "Caterpillar", "320-GC", new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_536.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_536.pdf", "Kazı", "/docs/invoice_536.pdf", new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-536", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_536.pdf", "Global Mining Tech", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 537, "RFID-EQ-537", 0m, "/docs/maint_537.pdf", 0m, "Aktif", "Kamyon TR-37", "Volvo", "FMX-460", new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_537.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_537.pdf", "Nakliye", "/docs/invoice_537.pdf", new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-537", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_537.pdf", "Global Mining Tech", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 538, "RFID-EQ-538", 0m, "/docs/maint_538.pdf", 0m, "Aktif", "Ekskavatör EX-38", "Caterpillar", "320-GC", new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_538.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_538.pdf", "Kazı", "/docs/invoice_538.pdf", new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-538", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_538.pdf", "Global Mining Tech", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 539, "RFID-EQ-539", 0m, "/docs/maint_539.pdf", 0m, "Aktif", "Kamyon TR-39", "Volvo", "FMX-460", new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_539.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_539.pdf", "Nakliye", "/docs/invoice_539.pdf", new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-539", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_539.pdf", "Global Mining Tech", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 540, "RFID-EQ-540", 0m, "/docs/maint_540.pdf", 0m, "Aktif", "Ekskavatör EX-40", "Caterpillar", "320-GC", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_540.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_540.pdf", "Kazı", "/docs/invoice_540.pdf", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-540", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_540.pdf", "Global Mining Tech", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 541, "RFID-EQ-541", 0m, "/docs/maint_541.pdf", 0m, "Aktif", "Kamyon TR-41", "Volvo", "FMX-460", new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_541.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_541.pdf", "Nakliye", "/docs/invoice_541.pdf", new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-541", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_541.pdf", "Global Mining Tech", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 542, "RFID-EQ-542", 0m, "/docs/maint_542.pdf", 0m, "Aktif", "Ekskavatör EX-42", "Caterpillar", "320-GC", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_542.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_542.pdf", "Kazı", "/docs/invoice_542.pdf", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-542", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_542.pdf", "Global Mining Tech", new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 543, "RFID-EQ-543", 0m, "/docs/maint_543.pdf", 0m, "Aktif", "Kamyon TR-43", "Volvo", "FMX-460", new DateTime(2024, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_543.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_543.pdf", "Nakliye", "/docs/invoice_543.pdf", new DateTime(2024, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-543", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_543.pdf", "Global Mining Tech", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 544, "RFID-EQ-544", 0m, "/docs/maint_544.pdf", 0m, "Aktif", "Ekskavatör EX-44", "Caterpillar", "320-GC", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_544.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_544.pdf", "Kazı", "/docs/invoice_544.pdf", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-544", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_544.pdf", "Global Mining Tech", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 545, "RFID-EQ-545", 0m, "/docs/maint_545.pdf", 0m, "Aktif", "Kamyon TR-45", "Volvo", "FMX-460", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_545.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_545.pdf", "Nakliye", "/docs/invoice_545.pdf", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-545", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_545.pdf", "Global Mining Tech", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 546, "RFID-EQ-546", 0m, "/docs/maint_546.pdf", 0m, "Aktif", "Ekskavatör EX-46", "Caterpillar", "320-GC", new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_546.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_546.pdf", "Kazı", "/docs/invoice_546.pdf", new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-546", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_546.pdf", "Global Mining Tech", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 547, "RFID-EQ-547", 0m, "/docs/maint_547.pdf", 0m, "Aktif", "Kamyon TR-47", "Volvo", "FMX-460", new DateTime(2024, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_547.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_547.pdf", "Nakliye", "/docs/invoice_547.pdf", new DateTime(2024, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-547", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_547.pdf", "Global Mining Tech", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 548, "RFID-EQ-548", 0m, "/docs/maint_548.pdf", 0m, "Aktif", "Ekskavatör EX-48", "Caterpillar", "320-GC", new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_548.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_548.pdf", "Kazı", "/docs/invoice_548.pdf", new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-548", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_548.pdf", "Global Mining Tech", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 549, "RFID-EQ-549", 0m, "/docs/maint_549.pdf", 0m, "Aktif", "Kamyon TR-49", "Volvo", "FMX-460", new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_549.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_549.pdf", "Nakliye", "/docs/invoice_549.pdf", new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-549", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_549.pdf", "Global Mining Tech", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 550, "RFID-EQ-550", 0m, "/docs/maint_550.pdf", 0m, "Aktif", "Ekskavatör EX-50", "Caterpillar", "320-GC", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty_550.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual_550.pdf", "Kazı", "/docs/invoice_550.pdf", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-2026-550", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/tech_550.pdf", "Global Mining Tech", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "personelId", "adres", "calisanTipi", "calismaKonumu", "cinsiyet", "departman", "dogumTarihi", "eposta", "ikinciTelNo", "iseGirisTarihi", "kullaniciRolu", "personelAdi", "personelDurumu", "personelSoyadi", "rfidKartNumarasi", "sifreHash", "sonGirisTarihi", "tckn", "telNo", "uzmanlik" },
                values: new object[,]
                {
                    { 1, "Mahalle_1, Sokak_1, No: 1 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel1@minemation.com", "05559999901", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Büşra", "Aktif", "Arslan", "RFID-101", "10000000001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000001", "05550000001", "Sistem Mühendisi" },
                    { 2, "Mahalle_2, Sokak_2, No: 2 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel2@minemation.com", "05559999902", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_2", "Aktif", "Soyad_2", "RFID-102", "10000000002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000002", "05550000002", "Operatör (Kamyon)" },
                    { 3, "Mahalle_3, Sokak_3, No: 3 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel3@minemation.com", "05559999903", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_3", "Aktif", "Soyad_3", "RFID-103", "10000000003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000003", "05550000003", "ISG Uzmanı" },
                    { 4, "Mahalle_4, Sokak_4, No: 4 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel4@minemation.com", "05559999904", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_4", "Aktif", "Soyad_4", "RFID-104", "10000000004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000004", "05550000004", "Jeoloji Mühendisi" },
                    { 5, "Mahalle_5, Sokak_5, No: 5 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel5@minemation.com", "05559999905", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_5", "Aktif", "Soyad_5", "RFID-105", "10000000005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000005", "05550000005", "Elektrik Teknisyeni" },
                    { 6, "Mahalle_6, Sokak_6, No: 6 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel6@minemation.com", "05559999906", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_6", "Aktif", "Soyad_6", "RFID-106", "10000000006", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000006", "05550000006", "Mekanik Bakım Ustası" },
                    { 7, "Mahalle_7, Sokak_7, No: 7 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel7@minemation.com", "05559999907", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_7", "Aktif", "Soyad_7", "RFID-107", "10000000007", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000007", "05550000007", "Vardiya Amiri" },
                    { 8, "Mahalle_8, Sokak_8, No: 8 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel8@minemation.com", "05559999908", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_8", "Aktif", "Soyad_8", "RFID-108", "10000000008", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000008", "05550000008", "Saha Formeni" },
                    { 9, "Mahalle_9, Sokak_9, No: 9 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel9@minemation.com", "05559999909", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_9", "Aktif", "Soyad_9", "RFID-109", "10000000009", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000009", "05550000009", "Patlayıcı Uzmanı" },
                    { 10, "Mahalle_10, Sokak_10, No: 10 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel10@minemation.com", "05559999910", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_10", "Aktif", "Soyad_10", "RFID-110", "10000000010", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000010", "05550000010", "Survey Mühendisi (Ölçüm)" },
                    { 11, "Mahalle_11, Sokak_11, No: 11 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel11@minemation.com", "05559999911", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_11", "Aktif", "Soyad_11", "RFID-111", "10000000011", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000011", "05550000011", "Maden Teknikeri" },
                    { 12, "Mahalle_12, Sokak_12, No: 12 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel12@minemation.com", "05559999912", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_12", "Aktif", "Soyad_12", "RFID-112", "10000000012", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000012", "05550000012", "Maden Mühendisi" },
                    { 13, "Mahalle_13, Sokak_13, No: 13 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel13@minemation.com", "05559999913", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_13", "Aktif", "Soyad_13", "RFID-113", "10000000013", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000013", "05550000013", "Operatör (Ekskavatör)" },
                    { 14, "Mahalle_14, Sokak_14, No: 14 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel14@minemation.com", "05559999914", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_14", "Aktif", "Soyad_14", "RFID-114", "10000000014", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000014", "05550000014", "Operatör (Kamyon)" },
                    { 15, "Mahalle_15, Sokak_15, No: 15 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel15@minemation.com", "05559999915", new DateTime(2024, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_15", "Aktif", "Soyad_15", "RFID-115", "10000000015", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000015", "05550000015", "ISG Uzmanı" },
                    { 16, "Mahalle_16, Sokak_16, No: 16 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel16@minemation.com", "05559999916", new DateTime(2024, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_16", "Aktif", "Soyad_16", "RFID-116", "10000000016", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000016", "05550000016", "Jeoloji Mühendisi" },
                    { 17, "Mahalle_17, Sokak_17, No: 17 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel17@minemation.com", "05559999917", new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_17", "Aktif", "Soyad_17", "RFID-117", "10000000017", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000017", "05550000017", "Elektrik Teknisyeni" },
                    { 18, "Mahalle_18, Sokak_18, No: 18 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel18@minemation.com", "05559999918", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_18", "Aktif", "Soyad_18", "RFID-118", "10000000018", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000018", "05550000018", "Mekanik Bakım Ustası" },
                    { 19, "Mahalle_19, Sokak_19, No: 19 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel19@minemation.com", "05559999919", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_19", "Aktif", "Soyad_19", "RFID-119", "10000000019", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000019", "05550000019", "Vardiya Amiri" },
                    { 20, "Mahalle_20, Sokak_20, No: 20 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel20@minemation.com", "05559999920", new DateTime(2024, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_20", "Aktif", "Soyad_20", "RFID-120", "10000000020", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000020", "05550000020", "Saha Formeni" },
                    { 21, "Mahalle_21, Sokak_21, No: 21 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel21@minemation.com", "05559999921", new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_21", "Aktif", "Soyad_21", "RFID-121", "10000000021", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000021", "05550000021", "Patlayıcı Uzmanı" },
                    { 22, "Mahalle_22, Sokak_22, No: 22 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel22@minemation.com", "05559999922", new DateTime(2024, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_22", "Aktif", "Soyad_22", "RFID-122", "10000000022", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000022", "05550000022", "Survey Mühendisi (Ölçüm)" },
                    { 23, "Mahalle_23, Sokak_23, No: 23 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel23@minemation.com", "05559999923", new DateTime(2024, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_23", "Aktif", "Soyad_23", "RFID-123", "10000000023", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000023", "05550000023", "Maden Teknikeri" },
                    { 24, "Mahalle_24, Sokak_24, No: 24 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel24@minemation.com", "05559999924", new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_24", "Aktif", "Soyad_24", "RFID-124", "10000000024", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000024", "05550000024", "Maden Mühendisi" },
                    { 25, "Mahalle_25, Sokak_25, No: 25 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel25@minemation.com", "05559999925", new DateTime(2024, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_25", "Aktif", "Soyad_25", "RFID-125", "10000000025", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000025", "05550000025", "Operatör (Ekskavatör)" },
                    { 26, "Mahalle_26, Sokak_26, No: 26 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel26@minemation.com", "05559999926", new DateTime(2024, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_26", "Aktif", "Soyad_26", "RFID-126", "10000000026", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000026", "05550000026", "Operatör (Kamyon)" },
                    { 27, "Mahalle_27, Sokak_27, No: 27 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel27@minemation.com", "05559999927", new DateTime(2024, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_27", "Aktif", "Soyad_27", "RFID-127", "10000000027", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000027", "05550000027", "ISG Uzmanı" },
                    { 28, "Mahalle_28, Sokak_28, No: 28 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel28@minemation.com", "05559999928", new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_28", "Aktif", "Soyad_28", "RFID-128", "10000000028", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000028", "05550000028", "Jeoloji Mühendisi" },
                    { 29, "Mahalle_29, Sokak_29, No: 29 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel29@minemation.com", "05559999929", new DateTime(2024, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_29", "Aktif", "Soyad_29", "RFID-129", "10000000029", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000029", "05550000029", "Elektrik Teknisyeni" },
                    { 30, "Mahalle_30, Sokak_30, No: 30 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel30@minemation.com", "05559999930", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_30", "Aktif", "Soyad_30", "RFID-130", "10000000030", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000030", "05550000030", "Mekanik Bakım Ustası" },
                    { 31, "Mahalle_31, Sokak_31, No: 31 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel31@minemation.com", "05559999931", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_31", "Aktif", "Soyad_31", "RFID-131", "10000000031", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000031", "05550000031", "Vardiya Amiri" },
                    { 32, "Mahalle_32, Sokak_32, No: 32 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel32@minemation.com", "05559999932", new DateTime(2024, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_32", "Aktif", "Soyad_32", "RFID-132", "10000000032", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000032", "05550000032", "Saha Formeni" },
                    { 33, "Mahalle_33, Sokak_33, No: 33 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel33@minemation.com", "05559999933", new DateTime(2024, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_33", "Aktif", "Soyad_33", "RFID-133", "10000000033", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000033", "05550000033", "Patlayıcı Uzmanı" },
                    { 34, "Mahalle_34, Sokak_34, No: 34 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel34@minemation.com", "05559999934", new DateTime(2024, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_34", "Aktif", "Soyad_34", "RFID-134", "10000000034", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000034", "05550000034", "Survey Mühendisi (Ölçüm)" },
                    { 35, "Mahalle_35, Sokak_35, No: 35 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel35@minemation.com", "05559999935", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_35", "Aktif", "Soyad_35", "RFID-135", "10000000035", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000035", "05550000035", "Maden Teknikeri" },
                    { 36, "Mahalle_36, Sokak_36, No: 36 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel36@minemation.com", "05559999936", new DateTime(2024, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_36", "Aktif", "Soyad_36", "RFID-136", "10000000036", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000036", "05550000036", "Maden Mühendisi" },
                    { 37, "Mahalle_37, Sokak_37, No: 37 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel37@minemation.com", "05559999937", new DateTime(2024, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_37", "Aktif", "Soyad_37", "RFID-137", "10000000037", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000037", "05550000037", "Operatör (Ekskavatör)" },
                    { 38, "Mahalle_38, Sokak_38, No: 38 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel38@minemation.com", "05559999938", new DateTime(2024, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_38", "Aktif", "Soyad_38", "RFID-138", "10000000038", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000038", "05550000038", "Operatör (Kamyon)" },
                    { 39, "Mahalle_39, Sokak_39, No: 39 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel39@minemation.com", "05559999939", new DateTime(2024, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_39", "Aktif", "Soyad_39", "RFID-139", "10000000039", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000039", "05550000039", "ISG Uzmanı" },
                    { 40, "Mahalle_40, Sokak_40, No: 40 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel40@minemation.com", "05559999940", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_40", "Aktif", "Soyad_40", "RFID-140", "10000000040", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000040", "05550000040", "Jeoloji Mühendisi" },
                    { 41, "Mahalle_41, Sokak_41, No: 41 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel41@minemation.com", "05559999941", new DateTime(2024, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_41", "Aktif", "Soyad_41", "RFID-141", "10000000041", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000041", "05550000041", "Elektrik Teknisyeni" },
                    { 42, "Mahalle_42, Sokak_42, No: 42 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel42@minemation.com", "05559999942", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_42", "Aktif", "Soyad_42", "RFID-142", "10000000042", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000042", "05550000042", "Mekanik Bakım Ustası" },
                    { 43, "Mahalle_43, Sokak_43, No: 43 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel43@minemation.com", "05559999943", new DateTime(2024, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_43", "Aktif", "Soyad_43", "RFID-143", "10000000043", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000043", "05550000043", "Vardiya Amiri" },
                    { 44, "Mahalle_44, Sokak_44, No: 44 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel44@minemation.com", "05559999944", new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_44", "Aktif", "Soyad_44", "RFID-144", "10000000044", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000044", "05550000044", "Saha Formeni" },
                    { 45, "Mahalle_45, Sokak_45, No: 45 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel45@minemation.com", "05559999945", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_45", "Aktif", "Soyad_45", "RFID-145", "10000000045", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000045", "05550000045", "Patlayıcı Uzmanı" },
                    { 46, "Mahalle_46, Sokak_46, No: 46 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel46@minemation.com", "05559999946", new DateTime(2024, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_46", "Aktif", "Soyad_46", "RFID-146", "10000000046", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000046", "05550000046", "Survey Mühendisi (Ölçüm)" },
                    { 47, "Mahalle_47, Sokak_47, No: 47 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel47@minemation.com", "05559999947", new DateTime(2024, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_47", "Aktif", "Soyad_47", "RFID-147", "10000000047", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000047", "05550000047", "Maden Teknikeri" },
                    { 48, "Mahalle_48, Sokak_48, No: 48 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Üretim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel48@minemation.com", "05559999948", new DateTime(2024, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator", "Ad_48", "Aktif", "Soyad_48", "RFID-148", "10000000048", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000048", "05550000048", "Maden Mühendisi" },
                    { 49, "Mahalle_49, Sokak_49, No: 49 Ankara", "Tam Zamanlı", "Saha-1", "Kadın", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel49@minemation.com", "05559999949", new DateTime(2024, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "User", "Ad_49", "Aktif", "Soyad_49", "RFID-149", "10000000049", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000049", "05550000049", "Operatör (Ekskavatör)" },
                    { 50, "Mahalle_50, Sokak_50, No: 50 Ankara", "Tam Zamanlı", "Saha-1", "Erkek", "Teknik", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "personel50@minemation.com", "05559999950", new DateTime(2024, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Ad_50", "Aktif", "Soyad_50", "RFID-150", "10000000050", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10000000050", "05550000050", "Operatör (Kamyon)" }
                });

            migrationBuilder.InsertData(
                table: "AcilDurumIletisim",
                columns: new[] { "acilDurumKisisiId", "acilDurumKisileriAd", "acilDurumKisileriSoyad", "acilDurumKisileriTelNo", "acilDurumKisileriYakinlik", "personelId" },
                values: new object[,]
                {
                    { 1, "Yakin_1", "Soyad_1", "05000000001", "Baba", 1 },
                    { 2, "Yakin_2", "Soyad_2", "05000000002", "Eş", 2 },
                    { 3, "Yakin_3", "Soyad_3", "05000000003", "Kardeş", 3 },
                    { 4, "Yakin_4", "Soyad_4", "05000000004", "Arkadaş", 4 },
                    { 5, "Yakin_5", "Soyad_5", "05000000005", "Anne", 5 },
                    { 6, "Yakin_6", "Soyad_6", "05000000006", "Baba", 6 },
                    { 7, "Yakin_7", "Soyad_7", "05000000007", "Eş", 7 },
                    { 8, "Yakin_8", "Soyad_8", "05000000008", "Kardeş", 8 },
                    { 9, "Yakin_9", "Soyad_9", "05000000009", "Arkadaş", 9 },
                    { 10, "Yakin_10", "Soyad_10", "05000000010", "Anne", 10 },
                    { 11, "Yakin_11", "Soyad_11", "05000000011", "Baba", 11 },
                    { 12, "Yakin_12", "Soyad_12", "05000000012", "Eş", 12 },
                    { 13, "Yakin_13", "Soyad_13", "05000000013", "Kardeş", 13 },
                    { 14, "Yakin_14", "Soyad_14", "05000000014", "Arkadaş", 14 },
                    { 15, "Yakin_15", "Soyad_15", "05000000015", "Anne", 15 },
                    { 16, "Yakin_16", "Soyad_16", "05000000016", "Baba", 16 },
                    { 17, "Yakin_17", "Soyad_17", "05000000017", "Eş", 17 },
                    { 18, "Yakin_18", "Soyad_18", "05000000018", "Kardeş", 18 },
                    { 19, "Yakin_19", "Soyad_19", "05000000019", "Arkadaş", 19 },
                    { 20, "Yakin_20", "Soyad_20", "05000000020", "Anne", 20 },
                    { 21, "Yakin_21", "Soyad_21", "05000000021", "Baba", 21 },
                    { 22, "Yakin_22", "Soyad_22", "05000000022", "Eş", 22 },
                    { 23, "Yakin_23", "Soyad_23", "05000000023", "Kardeş", 23 },
                    { 24, "Yakin_24", "Soyad_24", "05000000024", "Arkadaş", 24 },
                    { 25, "Yakin_25", "Soyad_25", "05000000025", "Anne", 25 },
                    { 26, "Yakin_26", "Soyad_26", "05000000026", "Baba", 26 },
                    { 27, "Yakin_27", "Soyad_27", "05000000027", "Eş", 27 },
                    { 28, "Yakin_28", "Soyad_28", "05000000028", "Kardeş", 28 },
                    { 29, "Yakin_29", "Soyad_29", "05000000029", "Arkadaş", 29 },
                    { 30, "Yakin_30", "Soyad_30", "05000000030", "Anne", 30 },
                    { 31, "Yakin_31", "Soyad_31", "05000000031", "Baba", 31 },
                    { 32, "Yakin_32", "Soyad_32", "05000000032", "Eş", 32 },
                    { 33, "Yakin_33", "Soyad_33", "05000000033", "Kardeş", 33 },
                    { 34, "Yakin_34", "Soyad_34", "05000000034", "Arkadaş", 34 },
                    { 35, "Yakin_35", "Soyad_35", "05000000035", "Anne", 35 },
                    { 36, "Yakin_36", "Soyad_36", "05000000036", "Baba", 36 },
                    { 37, "Yakin_37", "Soyad_37", "05000000037", "Eş", 37 },
                    { 38, "Yakin_38", "Soyad_38", "05000000038", "Kardeş", 38 },
                    { 39, "Yakin_39", "Soyad_39", "05000000039", "Arkadaş", 39 },
                    { 40, "Yakin_40", "Soyad_40", "05000000040", "Anne", 40 },
                    { 41, "Yakin_41", "Soyad_41", "05000000041", "Baba", 41 },
                    { 42, "Yakin_42", "Soyad_42", "05000000042", "Eş", 42 },
                    { 43, "Yakin_43", "Soyad_43", "05000000043", "Kardeş", 43 },
                    { 44, "Yakin_44", "Soyad_44", "05000000044", "Arkadaş", 44 },
                    { 45, "Yakin_45", "Soyad_45", "05000000045", "Anne", 45 },
                    { 46, "Yakin_46", "Soyad_46", "05000000046", "Baba", 46 },
                    { 47, "Yakin_47", "Soyad_47", "05000000047", "Eş", 47 },
                    { 48, "Yakin_48", "Soyad_48", "05000000048", "Kardeş", 48 },
                    { 49, "Yakin_49", "Soyad_49", "05000000049", "Arkadaş", 49 },
                    { 50, "Yakin_50", "Soyad_50", "05000000050", "Anne", 50 }
                });

            migrationBuilder.InsertData(
                table: "Ekskavator",
                columns: new[] { "ekipmanId", "bomUzunlugu", "kovaKapasitesi", "maksimumKaziDerinligi", "motorGucu", "paletTipi", "plaka" },
                values: new object[,]
                {
                    { 504, 0m, 5m, 0m, 400m, "Çelik", "06-BS-4" },
                    { 508, 0m, 5m, 0m, 400m, "Çelik", "06-BS-8" },
                    { 512, 0m, 5m, 0m, 400m, "Çelik", "06-BS-12" },
                    { 516, 0m, 5m, 0m, 400m, "Çelik", "06-BS-16" },
                    { 520, 0m, 5m, 0m, 400m, "Çelik", "06-BS-20" },
                    { 524, 0m, 5m, 0m, 400m, "Çelik", "06-BS-24" },
                    { 528, 0m, 5m, 0m, 400m, "Çelik", "06-BS-28" },
                    { 532, 0m, 5m, 0m, 400m, "Çelik", "06-BS-32" },
                    { 536, 0m, 5m, 0m, 400m, "Çelik", "06-BS-36" },
                    { 540, 0m, 5m, 0m, 400m, "Çelik", "06-BS-40" },
                    { 544, 0m, 5m, 0m, 400m, "Çelik", "06-BS-44" },
                    { 548, 0m, 5m, 0m, 400m, "Çelik", "06-BS-48" }
                });

            migrationBuilder.InsertData(
                table: "Hafriyat",
                columns: new[] { "ekipmanId", "azamiYukAgirligi", "damperHacmi", "dingilSayisi", "plaka" },
                values: new object[,]
                {
                    { 501, 30000m, 15m, 0, "06-AR-1" },
                    { 505, 30000m, 15m, 0, "06-AR-5" },
                    { 509, 30000m, 15m, 0, "06-AR-9" },
                    { 513, 30000m, 15m, 0, "06-AR-13" },
                    { 517, 30000m, 15m, 0, "06-AR-17" },
                    { 521, 30000m, 15m, 0, "06-AR-21" },
                    { 525, 30000m, 15m, 0, "06-AR-25" },
                    { 529, 30000m, 15m, 0, "06-AR-29" },
                    { 533, 30000m, 15m, 0, "06-AR-33" },
                    { 537, 30000m, 15m, 0, "06-AR-37" },
                    { 541, 30000m, 15m, 0, "06-AR-41" },
                    { 545, 30000m, 15m, 0, "06-AR-45" },
                    { 549, 30000m, 15m, 0, "06-AR-49" }
                });

            migrationBuilder.InsertData(
                table: "Kepce",
                columns: new[] { "ekipmanId", "bosaltmaYuksekligi", "devrilmeYuku", "kovaKapasitesi", "plaka", "yuklemeKapasitesi" },
                values: new object[,]
                {
                    { 502, 0m, 0m, 4m, "06-MN-2", 8m },
                    { 506, 0m, 0m, 4m, "06-MN-6", 8m },
                    { 510, 0m, 0m, 4m, "06-MN-10", 8m },
                    { 514, 0m, 0m, 4m, "06-MN-14", 8m },
                    { 518, 0m, 0m, 4m, "06-MN-18", 8m },
                    { 522, 0m, 0m, 4m, "06-MN-22", 8m },
                    { 526, 0m, 0m, 4m, "06-MN-26", 8m },
                    { 530, 0m, 0m, 4m, "06-MN-30", 8m },
                    { 534, 0m, 0m, 4m, "06-MN-34", 8m },
                    { 538, 0m, 0m, 4m, "06-MN-38", 8m },
                    { 542, 0m, 0m, 4m, "06-MN-42", 8m },
                    { 546, 0m, 0m, 4m, "06-MN-46", 8m },
                    { 550, 0m, 0m, 4m, "06-MN-50", 8m }
                });

            migrationBuilder.InsertData(
                table: "Kirici",
                columns: new[] { "ekipmanId", "calismaBasinci", "dakikadakiDarbeSayisi", "darbeEnerjisi", "gerekenYagDebisi", "ucTipi" },
                values: new object[,]
                {
                    { 503, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 507, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 511, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 515, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 519, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 523, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 527, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 531, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 535, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 539, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 543, 0m, 0m, 0m, "50L/dk", "Sivri" },
                    { 547, 0m, 0m, 0m, "50L/dk", "Sivri" }
                });

            migrationBuilder.InsertData(
                table: "Rapor",
                columns: new[] { "raporId", "ekipmanId", "personelId", "raporAciklamasi", "raporAdi", "raporDosyaYolu", "raporOlusturmaTarihi", "raporTuru", "zamanAraligi" },
                values: new object[,]
                {
                    { 1, null, 2, "Rapor içeriği detayları 1", "Rapor_1", "/reports/rep_1.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 2, null, 3, "Rapor içeriği detayları 2", "Rapor_2", "/reports/rep_2.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 3, null, 4, "Rapor içeriği detayları 3", "Rapor_3", "/reports/rep_3.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 4, null, 5, "Rapor içeriği detayları 4", "Rapor_4", "/reports/rep_4.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 5, null, 6, "Rapor içeriği detayları 5", "Rapor_5", "/reports/rep_5.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 6, null, 7, "Rapor içeriği detayları 6", "Rapor_6", "/reports/rep_6.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 7, null, 8, "Rapor içeriği detayları 7", "Rapor_7", "/reports/rep_7.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 8, null, 9, "Rapor içeriği detayları 8", "Rapor_8", "/reports/rep_8.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 9, null, 10, "Rapor içeriği detayları 9", "Rapor_9", "/reports/rep_9.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 10, null, 11, "Rapor içeriği detayları 10", "Rapor_10", "/reports/rep_10.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 11, null, 12, "Rapor içeriği detayları 11", "Rapor_11", "/reports/rep_11.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 12, null, 13, "Rapor içeriği detayları 12", "Rapor_12", "/reports/rep_12.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 13, null, 14, "Rapor içeriği detayları 13", "Rapor_13", "/reports/rep_13.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 14, null, 15, "Rapor içeriği detayları 14", "Rapor_14", "/reports/rep_14.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 15, null, 16, "Rapor içeriği detayları 15", "Rapor_15", "/reports/rep_15.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 16, null, 17, "Rapor içeriği detayları 16", "Rapor_16", "/reports/rep_16.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 17, null, 18, "Rapor içeriği detayları 17", "Rapor_17", "/reports/rep_17.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 18, null, 19, "Rapor içeriği detayları 18", "Rapor_18", "/reports/rep_18.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 19, null, 20, "Rapor içeriği detayları 19", "Rapor_19", "/reports/rep_19.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 20, null, 21, "Rapor içeriği detayları 20", "Rapor_20", "/reports/rep_20.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 21, null, 22, "Rapor içeriği detayları 21", "Rapor_21", "/reports/rep_21.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 22, null, 23, "Rapor içeriği detayları 22", "Rapor_22", "/reports/rep_22.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 23, null, 24, "Rapor içeriği detayları 23", "Rapor_23", "/reports/rep_23.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 24, null, 25, "Rapor içeriği detayları 24", "Rapor_24", "/reports/rep_24.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 25, null, 26, "Rapor içeriği detayları 25", "Rapor_25", "/reports/rep_25.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 26, null, 27, "Rapor içeriği detayları 26", "Rapor_26", "/reports/rep_26.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 27, null, 28, "Rapor içeriği detayları 27", "Rapor_27", "/reports/rep_27.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 28, null, 29, "Rapor içeriği detayları 28", "Rapor_28", "/reports/rep_28.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 29, null, 30, "Rapor içeriği detayları 29", "Rapor_29", "/reports/rep_29.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 30, null, 31, "Rapor içeriği detayları 30", "Rapor_30", "/reports/rep_30.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 31, null, 32, "Rapor içeriği detayları 31", "Rapor_31", "/reports/rep_31.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 32, null, 33, "Rapor içeriği detayları 32", "Rapor_32", "/reports/rep_32.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 33, null, 34, "Rapor içeriği detayları 33", "Rapor_33", "/reports/rep_33.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 34, null, 35, "Rapor içeriği detayları 34", "Rapor_34", "/reports/rep_34.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 35, null, 36, "Rapor içeriği detayları 35", "Rapor_35", "/reports/rep_35.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 36, null, 37, "Rapor içeriği detayları 36", "Rapor_36", "/reports/rep_36.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 37, null, 38, "Rapor içeriği detayları 37", "Rapor_37", "/reports/rep_37.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 38, null, 39, "Rapor içeriği detayları 38", "Rapor_38", "/reports/rep_38.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 39, null, 40, "Rapor içeriği detayları 39", "Rapor_39", "/reports/rep_39.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 40, null, 41, "Rapor içeriği detayları 40", "Rapor_40", "/reports/rep_40.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 41, null, 42, "Rapor içeriği detayları 41", "Rapor_41", "/reports/rep_41.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 42, null, 43, "Rapor içeriği detayları 42", "Rapor_42", "/reports/rep_42.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 43, null, 44, "Rapor içeriği detayları 43", "Rapor_43", "/reports/rep_43.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 44, null, 45, "Rapor içeriği detayları 44", "Rapor_44", "/reports/rep_44.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 45, null, 46, "Rapor içeriği detayları 45", "Rapor_45", "/reports/rep_45.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 46, null, 47, "Rapor içeriği detayları 46", "Rapor_46", "/reports/rep_46.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 47, null, 48, "Rapor içeriği detayları 47", "Rapor_47", "/reports/rep_47.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 48, null, 49, "Rapor içeriği detayları 48", "Rapor_48", "/reports/rep_48.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 49, null, 50, "Rapor içeriği detayları 49", "Rapor_49", "/reports/rep_49.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" },
                    { 50, null, 1, "Rapor içeriği detayları 50", "Rapor_50", "/reports/rep_50.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik", "Günlük" },
                    { 51, null, 2, "Rapor içeriği detayları 51", "Rapor_51", "/reports/rep_51.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İdari", "Günlük" }
                });

            migrationBuilder.InsertData(
                table: "SaglikBilgileri",
                columns: new[] { "personelId", "acilDurumNotu", "alerjiler", "kanGrubu", "kronikHastaliklar", "saglikCalismaKisitlamalari", "saglikDurumu", "sonMuayeneTarihi" },
                values: new object[,]
                {
                    { 1, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "B Rh+", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 50, "Bilinen kronik bir rahatsızlığı bulunmamaktadır.", "[\"Yok\"]", "0 Rh-", "[]", "Yok", "Genel sağlık durumu yerinde.", new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Sensor",
                columns: new[] { "ekipmanId", "baglantiProtokolu", "haberlesmeTipi", "hassasiyet", "maxEsikDeger", "minEsikDeger", "sensorDurumu", "sensorTipi" },
                values: new object[,]
                {
                    { 501, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 502, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 503, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 504, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 505, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 506, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 507, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 508, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 509, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 510, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 511, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 512, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 513, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 514, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 515, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 516, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 517, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 518, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 519, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 520, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 521, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 522, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 523, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 524, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 525, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 526, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 527, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 528, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 529, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 530, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 531, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 532, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 533, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 534, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 535, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 536, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 537, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 538, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 539, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 540, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 541, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 542, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 543, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 544, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 545, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 546, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 547, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 548, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" },
                    { 549, "LoRaWAN", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Sıcaklık" },
                    { 550, "MQTT", "Kablosuz", 0.10000000000000001, 100.0, 0.0, "Aktif", "Gaz" }
                });

            migrationBuilder.InsertData(
                table: "TakipCihazi",
                columns: new[] { "takipCihaziId", "ekipmanId", "personelId", "pilSeviyesi", "takipCihaziDurumu", "takipCihaziHaberlesmeProtokolu", "takipCihaziModeli", "takipCihaziSeriNo", "takipCihaziSonBaglantiZamani", "takipCihaziTuru" },
                values: new object[,]
                {
                    { 1, null, 1, 99.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 2, null, 2, 98.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-2", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 3, null, 3, 97.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-3", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 4, null, 4, 96.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 5, null, 5, 95.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-5", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 6, null, 6, 94.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-6", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 7, null, 7, 93.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-7", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 8, null, 8, 92.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 9, null, 9, 91.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 10, null, 10, 90.0m, "Bakımda", "UHF-RFID", "MineTrack-v1", "TC-10", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 11, null, 11, 89.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-11", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 12, null, 12, 88.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-12", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 13, null, 13, 87.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-13", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 14, null, 14, 86.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-14", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 15, null, 15, 85.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-15", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 16, null, 16, 84.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-16", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 17, null, 17, 83.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-17", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 18, null, 18, 82.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-18", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 19, null, 19, 81.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-19", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 20, null, 20, 80.0m, "Bakımda", "UHF-RFID", "MineTrack-v1", "TC-20", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 21, null, 21, 79.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-21", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 22, null, 22, 78.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-22", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 23, null, 23, 77.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-23", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 24, null, 24, 76.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-24", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 25, null, 25, 75.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-25", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 26, null, 26, 74.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-26", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 27, null, 27, 73.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-27", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 28, null, 28, 72.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-28", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 29, null, 29, 71.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-29", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 30, null, 30, 70.0m, "Bakımda", "UHF-RFID", "MineTrack-v1", "TC-30", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 31, null, 31, 69.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-31", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 32, null, 32, 68.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-32", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 33, null, 33, 67.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-33", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 34, null, 34, 66.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-34", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 35, null, 35, 65.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-35", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 36, null, 36, 64.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-36", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 37, null, 37, 63.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-37", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 38, null, 38, 62.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-38", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 39, null, 39, 61.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-39", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 40, null, 40, 100.0m, "Bakımda", "UHF-RFID", "MineTrack-v1", "TC-40", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 41, null, 41, 99.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-41", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 42, null, 42, 98.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-42", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 43, null, 43, 97.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-43", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 44, null, 44, 96.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-44", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 45, null, 45, 95.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-45", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 46, null, 46, 94.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-46", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" },
                    { 47, null, 47, 93.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-47", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Taşınabilir Beacon" },
                    { 48, null, 48, 92.0m, "Aktif", "UHF-RFID", "MineTrack-v1", "TC-48", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Personel Bilekliği" },
                    { 49, null, 49, 91.0m, "Aktif", "LoRaWAN", "SafeZone-v4", "TC-49", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kask Sensörü" },
                    { 50, null, 50, 90.0m, "Bakımda", "UHF-RFID", "MineTrack-v1", "TC-50", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Araç Takip Birimi" }
                });

            migrationBuilder.InsertData(
                table: "Vaka",
                columns: new[] { "vakaId", "ilgiliEkipmanId", "olayNedeni", "personelId", "raporlayanEkipmanId", "vakaAciklamasi", "vakaAdi", "vakaCiddiyetSeviyesi", "vakaDurumu", "vakaKapanmaTarihi", "vakaOlusmaTarihi", "vakaTuru" },
                values: new object[,]
                {
                    { 1, 501, "Yetersiz Havalandırma", 2, null, "Detaylı vaka açıklaması 1", "Vaka_1", "Orta", "Açık", null, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 2, 502, "Aşırı Yüklenme", 3, null, "Detaylı vaka açıklaması 2", "Vaka_2", "Orta", "Açık", null, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 3, 503, "Protokol İhlali", 4, null, "Detaylı vaka açıklaması 3", "Vaka_3", "Orta", "Açık", null, new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 4, 504, "Donanım Arızası", 5, null, "Detaylı vaka açıklaması 4", "Vaka_4", "Orta", "Açık", null, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 5, 505, "Sensör Kalibrasyon Hatası", 6, null, "Detaylı vaka açıklaması 5", "Vaka_5", "Orta", "Açık", null, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 6, 506, "Yetersiz Havalandırma", 7, null, "Detaylı vaka açıklaması 6", "Vaka_6", "Orta", "Açık", null, new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 7, 507, "Aşırı Yüklenme", 8, null, "Detaylı vaka açıklaması 7", "Vaka_7", "Orta", "Açık", null, new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 8, 508, "Protokol İhlali", 9, null, "Detaylı vaka açıklaması 8", "Vaka_8", "Orta", "Açık", null, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 9, 509, "Donanım Arızası", 10, null, "Detaylı vaka açıklaması 9", "Vaka_9", "Orta", "Açık", null, new DateTime(2025, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 10, 510, "Sensör Kalibrasyon Hatası", 11, null, "Detaylı vaka açıklaması 10", "Vaka_10", "Orta", "Açık", null, new DateTime(2025, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 11, 511, "Yetersiz Havalandırma", 12, null, "Detaylı vaka açıklaması 11", "Vaka_11", "Orta", "Açık", null, new DateTime(2025, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 12, 512, "Aşırı Yüklenme", 13, null, "Detaylı vaka açıklaması 12", "Vaka_12", "Orta", "Açık", null, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 13, 513, "Protokol İhlali", 14, null, "Detaylı vaka açıklaması 13", "Vaka_13", "Orta", "Açık", null, new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 14, 514, "Donanım Arızası", 15, null, "Detaylı vaka açıklaması 14", "Vaka_14", "Orta", "Açık", null, new DateTime(2025, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 15, 515, "Sensör Kalibrasyon Hatası", 16, null, "Detaylı vaka açıklaması 15", "Vaka_15", "Orta", "Açık", null, new DateTime(2025, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 16, 516, "Yetersiz Havalandırma", 17, null, "Detaylı vaka açıklaması 16", "Vaka_16", "Orta", "Açık", null, new DateTime(2025, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 17, 517, "Aşırı Yüklenme", 18, null, "Detaylı vaka açıklaması 17", "Vaka_17", "Orta", "Açık", null, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 18, 518, "Protokol İhlali", 19, null, "Detaylı vaka açıklaması 18", "Vaka_18", "Orta", "Açık", null, new DateTime(2025, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 19, 519, "Donanım Arızası", 20, null, "Detaylı vaka açıklaması 19", "Vaka_19", "Orta", "Açık", null, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 20, 520, "Sensör Kalibrasyon Hatası", 21, null, "Detaylı vaka açıklaması 20", "Vaka_20", "Orta", "Açık", null, new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 21, 521, "Yetersiz Havalandırma", 22, null, "Detaylı vaka açıklaması 21", "Vaka_21", "Orta", "Açık", null, new DateTime(2025, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 22, 522, "Aşırı Yüklenme", 23, null, "Detaylı vaka açıklaması 22", "Vaka_22", "Orta", "Açık", null, new DateTime(2025, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 23, 523, "Protokol İhlali", 24, null, "Detaylı vaka açıklaması 23", "Vaka_23", "Orta", "Açık", null, new DateTime(2025, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 24, 524, "Donanım Arızası", 25, null, "Detaylı vaka açıklaması 24", "Vaka_24", "Orta", "Açık", null, new DateTime(2025, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 25, 525, "Sensör Kalibrasyon Hatası", 26, null, "Detaylı vaka açıklaması 25", "Vaka_25", "Orta", "Açık", null, new DateTime(2025, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 26, 526, "Yetersiz Havalandırma", 27, null, "Detaylı vaka açıklaması 26", "Vaka_26", "Orta", "Açık", null, new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 27, 527, "Aşırı Yüklenme", 28, null, "Detaylı vaka açıklaması 27", "Vaka_27", "Orta", "Açık", null, new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 28, 528, "Protokol İhlali", 29, null, "Detaylı vaka açıklaması 28", "Vaka_28", "Orta", "Açık", null, new DateTime(2025, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 29, 529, "Donanım Arızası", 30, null, "Detaylı vaka açıklaması 29", "Vaka_29", "Orta", "Açık", null, new DateTime(2025, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 30, 530, "Sensör Kalibrasyon Hatası", 31, null, "Detaylı vaka açıklaması 30", "Vaka_30", "Orta", "Açık", null, new DateTime(2025, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 31, 531, "Yetersiz Havalandırma", 32, null, "Detaylı vaka açıklaması 31", "Vaka_31", "Orta", "Açık", null, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 32, 532, "Aşırı Yüklenme", 33, null, "Detaylı vaka açıklaması 32", "Vaka_32", "Orta", "Açık", null, new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 33, 533, "Protokol İhlali", 34, null, "Detaylı vaka açıklaması 33", "Vaka_33", "Orta", "Açık", null, new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 34, 534, "Donanım Arızası", 35, null, "Detaylı vaka açıklaması 34", "Vaka_34", "Orta", "Açık", null, new DateTime(2025, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 35, 535, "Sensör Kalibrasyon Hatası", 36, null, "Detaylı vaka açıklaması 35", "Vaka_35", "Orta", "Açık", null, new DateTime(2025, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 36, 536, "Yetersiz Havalandırma", 37, null, "Detaylı vaka açıklaması 36", "Vaka_36", "Orta", "Açık", null, new DateTime(2025, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 37, 537, "Aşırı Yüklenme", 38, null, "Detaylı vaka açıklaması 37", "Vaka_37", "Orta", "Açık", null, new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 38, 538, "Protokol İhlali", 39, null, "Detaylı vaka açıklaması 38", "Vaka_38", "Orta", "Açık", null, new DateTime(2025, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 39, 539, "Donanım Arızası", 40, null, "Detaylı vaka açıklaması 39", "Vaka_39", "Orta", "Açık", null, new DateTime(2025, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 40, 540, "Sensör Kalibrasyon Hatası", 41, null, "Detaylı vaka açıklaması 40", "Vaka_40", "Orta", "Açık", null, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 41, 541, "Yetersiz Havalandırma", 42, null, "Detaylı vaka açıklaması 41", "Vaka_41", "Orta", "Açık", null, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 42, 542, "Aşırı Yüklenme", 43, null, "Detaylı vaka açıklaması 42", "Vaka_42", "Orta", "Açık", null, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 43, 543, "Protokol İhlali", 44, null, "Detaylı vaka açıklaması 43", "Vaka_43", "Orta", "Açık", null, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" },
                    { 44, 544, "Donanım Arızası", 45, null, "Detaylı vaka açıklaması 44", "Vaka_44", "Orta", "Açık", null, new DateTime(2025, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Arızası" },
                    { 45, 545, "Sensör Kalibrasyon Hatası", 46, null, "Detaylı vaka açıklaması 45", "Vaka_45", "Orta", "Açık", null, new DateTime(2025, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "İzinsiz Giriş" },
                    { 46, 546, "Yetersiz Havalandırma", 47, null, "Detaylı vaka açıklaması 46", "Vaka_46", "Orta", "Açık", null, new DateTime(2025, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" },
                    { 47, 547, "Aşırı Yüklenme", 48, null, "Detaylı vaka açıklaması 47", "Vaka_47", "Orta", "Açık", null, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Güvenlik İhlali" },
                    { 48, 548, "Protokol İhlali", 49, null, "Detaylı vaka açıklaması 48", "Vaka_48", "Orta", "Açık", null, new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ekipman Kaybı" },
                    { 49, 549, "Donanım Arızası", 50, null, "Detaylı vaka açıklaması 49", "Vaka_49", "Orta", "Açık", null, new DateTime(2025, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gaz Sızıntısı" },
                    { 50, 550, "Sensör Kalibrasyon Hatası", 1, null, "Detaylı vaka açıklaması 50", "Vaka_50", "Orta", "Açık", null, new DateTime(2025, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yüksek Isı" }
                });

            migrationBuilder.InsertData(
                table: "Vardiya",
                columns: new[] { "vardiyaId", "calismaBolgesi", "ekipSayisi", "ekipmanId", "ekipmanOperatoru", "ekipmanSayisi", "operasyonRiskSeviyesi", "operasyonTipi", "personelSayisi", "toplaVardiyaSuresi", "vardiyaAdi", "vardiyaBaslangicTarihi", "vardiyaBitisTarihi", "vardiyaDurumu", "vardiyaIsgSorumlusu", "vardiyaNotlari", "vardiyaOlusturmaTarihi", "vardiyaSorumlusu", "vardiyaSupervizoru", "vardiyaTanimi", "vardiyaTeknikSorumlusu", "vardiyaTipi" },
                values: new object[,]
                {
                    { 1, "Güney Panosu", 0, null, "Mehmet Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-01", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Elif Nisa Okur ", "Güney Panosu Tahkimat Çalışması", 1, "Gündüz" },
                    { 2, "Batı Nakliye Hattı", 0, null, "Selin Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-02", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Berke Koyuncu", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Akşam" },
                    { 3, "Ana Üretim Katı", 0, null, "Can Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-03", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Büşra Arslan", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gece" },
                    { 4, "Havalandırma Şaftı", 0, null, "Ahmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-04", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Elif Nisa Okur ", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Akşam" },
                    { 5, "Kuzey Galerisi", 0, null, "Mehmet Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-05", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Berke Koyuncu", "Kuzey Galerisi Kazı Çalışması", 1, "Gündüz" },
                    { 6, "Güney Panosu", 0, null, "Selin Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-06", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Büşra Arslan", "Güney Panosu Tahkimat Çalışması", 1, "Gece" },
                    { 7, "Batı Nakliye Hattı", 0, null, "Can Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-07", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Elif Nisa Okur ", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gündüz" },
                    { 8, "Ana Üretim Katı", 0, null, "Ahmet Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-08", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, "Berke Koyuncu", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Akşam" },
                    { 9, "Havalandırma Şaftı", 0, null, "Mehmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-09", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, "Büşra Arslan", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gece" },
                    { 10, "Kuzey Galerisi", 0, null, "Selin Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-10", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, "Elif Nisa Okur ", "Kuzey Galerisi Kazı Çalışması", 1, "Akşam" },
                    { 11, "Güney Panosu", 0, null, "Can Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-11", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, "Berke Koyuncu", "Güney Panosu Tahkimat Çalışması", 1, "Gündüz" },
                    { 12, "Batı Nakliye Hattı", 0, null, "Ahmet Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-12", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, "Büşra Arslan", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gece" },
                    { 13, "Ana Üretim Katı", 0, null, "Mehmet Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-13", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, "Elif Nisa Okur ", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gündüz" },
                    { 14, "Havalandırma Şaftı", 0, null, "Selin Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-14", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, "Berke Koyuncu", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Akşam" },
                    { 15, "Kuzey Galerisi", 0, null, "Can Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-15", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, "Büşra Arslan", "Kuzey Galerisi Kazı Çalışması", 1, "Gece" },
                    { 16, "Güney Panosu", 0, null, "Ahmet Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-16", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, "Elif Nisa Okur ", "Güney Panosu Tahkimat Çalışması", 1, "Akşam" },
                    { 17, "Batı Nakliye Hattı", 0, null, "Mehmet Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-17", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, "Berke Koyuncu", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gündüz" },
                    { 18, "Ana Üretim Katı", 0, null, "Selin Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-18", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, "Büşra Arslan", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gece" },
                    { 19, "Havalandırma Şaftı", 0, null, "Can Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-19", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, "Elif Nisa Okur ", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gündüz" },
                    { 20, "Kuzey Galerisi", 0, null, "Ahmet Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-20", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, "Berke Koyuncu", "Kuzey Galerisi Kazı Çalışması", 1, "Akşam" },
                    { 21, "Güney Panosu", 0, null, "Mehmet Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-21", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, "Büşra Arslan", "Güney Panosu Tahkimat Çalışması", 1, "Gece" },
                    { 22, "Batı Nakliye Hattı", 0, null, "Selin Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-22", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, "Elif Nisa Okur ", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Akşam" },
                    { 23, "Ana Üretim Katı", 0, null, "Can Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-23", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, "Berke Koyuncu", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gündüz" },
                    { 24, "Havalandırma Şaftı", 0, null, "Ahmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-24", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, "Büşra Arslan", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gece" },
                    { 25, "Kuzey Galerisi", 0, null, "Mehmet Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-25", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, "Elif Nisa Okur ", "Kuzey Galerisi Kazı Çalışması", 1, "Gündüz" },
                    { 26, "Güney Panosu", 0, null, "Selin Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-26", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, "Berke Koyuncu", "Güney Panosu Tahkimat Çalışması", 1, "Akşam" },
                    { 27, "Batı Nakliye Hattı", 0, null, "Can Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-27", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, "Büşra Arslan", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gece" },
                    { 28, "Ana Üretim Katı", 0, null, "Ahmet Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-28", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, "Elif Nisa Okur ", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Akşam" },
                    { 29, "Havalandırma Şaftı", 0, null, "Mehmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-29", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, "Berke Koyuncu", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gündüz" },
                    { 30, "Kuzey Galerisi", 0, null, "Selin Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-30", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, "Büşra Arslan", "Kuzey Galerisi Kazı Çalışması", 1, "Gece" },
                    { 31, "Güney Panosu", 0, null, "Can Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-31", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, "Elif Nisa Okur ", "Güney Panosu Tahkimat Çalışması", 1, "Gündüz" },
                    { 32, "Batı Nakliye Hattı", 0, null, "Ahmet Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-32", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, "Berke Koyuncu", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Akşam" },
                    { 33, "Ana Üretim Katı", 0, null, "Mehmet Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-33", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, "Büşra Arslan", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gece" },
                    { 34, "Havalandırma Şaftı", 0, null, "Selin Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-34", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Elif Nisa Okur ", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Akşam" },
                    { 35, "Kuzey Galerisi", 0, null, "Can Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-35", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, "Berke Koyuncu", "Kuzey Galerisi Kazı Çalışması", 1, "Gündüz" },
                    { 36, "Güney Panosu", 0, null, "Ahmet Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-36", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, "Büşra Arslan", "Güney Panosu Tahkimat Çalışması", 1, "Gece" },
                    { 37, "Batı Nakliye Hattı", 0, null, "Mehmet Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-37", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, "Elif Nisa Okur ", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gündüz" },
                    { 38, "Ana Üretim Katı", 0, null, "Selin Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-38", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, "Berke Koyuncu", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Akşam" },
                    { 39, "Havalandırma Şaftı", 0, null, "Can Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-39", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, "Büşra Arslan", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gece" },
                    { 40, "Kuzey Galerisi", 0, null, "Ahmet Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-40", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, "Elif Nisa Okur ", "Kuzey Galerisi Kazı Çalışması", 1, "Akşam" },
                    { 41, "Güney Panosu", 0, null, "Mehmet Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-41", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, "Berke Koyuncu", "Güney Panosu Tahkimat Çalışması", 1, "Gündüz" },
                    { 42, "Batı Nakliye Hattı", 0, null, "Selin Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-42", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, "Büşra Arslan", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gece" },
                    { 43, "Ana Üretim Katı", 0, null, "Can Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-43", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 44, "Elif Nisa Okur ", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gündüz" },
                    { 44, "Havalandırma Şaftı", 0, null, "Ahmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-44", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, "Berke Koyuncu", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Akşam" },
                    { 45, "Kuzey Galerisi", 0, null, "Mehmet Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-45", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, "Büşra Arslan", "Kuzey Galerisi Kazı Çalışması", 1, "Gece" },
                    { 46, "Güney Panosu", 0, null, "Selin Operatör", 0, "Normal", "Tahkimat", 0, 0, "Vardiya-46", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "İlerleme planlandığı gibi gidiyor.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 47, "Elif Nisa Okur ", "Güney Panosu Tahkimat Çalışması", 1, "Akşam" },
                    { 47, "Batı Nakliye Hattı", 0, null, "Can Operatör", 0, "Normal", "Nakliye", 0, 0, "Vardiya-47", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin nemi normalin biraz üzerinde, dikkatli olunmalı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, "Berke Koyuncu", "Batı Nakliye Hattı Nakliye Çalışması", 1, "Gündüz" },
                    { 48, "Ana Üretim Katı", 0, null, "Ahmet Operatör", 0, "Normal", "Arama-Tarama", 0, 0, "Vardiya-48", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Haberleşme testleri başarılı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 49, "Büşra Arslan", "Ana Üretim Katı Arama-Tarama Çalışması", 1, "Gece" },
                    { 49, "Havalandırma Şaftı", 0, null, "Mehmet Operatör", 0, "Normal", "Bakım-Onarım", 0, 0, "Vardiya-49", new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Vardiya devri sorunsuz yapıldı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, "Elif Nisa Okur ", "Havalandırma Şaftı Bakım-Onarım Çalışması", 1, "Gündüz" },
                    { 50, "Kuzey Galerisi", 0, null, "Selin Operatör", 0, "Yüksek", "Kazı", 0, 0, "Vardiya-50", new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Ekipman bakımı tamamlandı.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Berke Koyuncu", "Kuzey Galerisi Kazı Çalışması", 1, "Akşam" }
                });

            migrationBuilder.InsertData(
                table: "Ekip",
                columns: new[] { "ekipId", "durum", "ekipUyeSayisi", "personelGorevi", "personelId", "vardiyaId" },
                values: new object[,]
                {
                    { 1, "Görevde", 0, "Güvenlik Sorumlusu", 1, 1 },
                    { 2, "Görevde", 0, "Operatör", 2, 2 },
                    { 3, "Görevde", 0, "Teknik Destek", 3, 3 },
                    { 4, "Görevde", 0, "ISG Gözlemci", 4, 4 },
                    { 5, "Görevde", 0, "Vardiya Amiri", 5, 5 },
                    { 6, "Görevde", 0, "Güvenlik Sorumlusu", 6, 6 },
                    { 7, "Görevde", 0, "Operatör", 7, 7 },
                    { 8, "Görevde", 0, "Teknik Destek", 8, 8 },
                    { 9, "Görevde", 0, "ISG Gözlemci", 9, 9 },
                    { 10, "Görevde", 0, "Vardiya Amiri", 10, 10 },
                    { 11, "Görevde", 0, "Güvenlik Sorumlusu", 11, 11 },
                    { 12, "Görevde", 0, "Operatör", 12, 12 },
                    { 13, "Görevde", 0, "Teknik Destek", 13, 13 },
                    { 14, "Görevde", 0, "ISG Gözlemci", 14, 14 },
                    { 15, "Görevde", 0, "Vardiya Amiri", 15, 15 },
                    { 16, "Görevde", 0, "Güvenlik Sorumlusu", 16, 16 },
                    { 17, "Görevde", 0, "Operatör", 17, 17 },
                    { 18, "Görevde", 0, "Teknik Destek", 18, 18 },
                    { 19, "Görevde", 0, "ISG Gözlemci", 19, 19 },
                    { 20, "Görevde", 0, "Vardiya Amiri", 20, 20 },
                    { 21, "Görevde", 0, "Güvenlik Sorumlusu", 21, 21 },
                    { 22, "Görevde", 0, "Operatör", 22, 22 },
                    { 23, "Görevde", 0, "Teknik Destek", 23, 23 },
                    { 24, "Görevde", 0, "ISG Gözlemci", 24, 24 },
                    { 25, "Görevde", 0, "Vardiya Amiri", 25, 25 },
                    { 26, "Görevde", 0, "Güvenlik Sorumlusu", 26, 26 },
                    { 27, "Görevde", 0, "Operatör", 27, 27 },
                    { 28, "Görevde", 0, "Teknik Destek", 28, 28 },
                    { 29, "Görevde", 0, "ISG Gözlemci", 29, 29 },
                    { 30, "Görevde", 0, "Vardiya Amiri", 30, 30 },
                    { 31, "Görevde", 0, "Güvenlik Sorumlusu", 31, 31 },
                    { 32, "Görevde", 0, "Operatör", 32, 32 },
                    { 33, "Görevde", 0, "Teknik Destek", 33, 33 },
                    { 34, "Görevde", 0, "ISG Gözlemci", 34, 34 },
                    { 35, "Görevde", 0, "Vardiya Amiri", 35, 35 },
                    { 36, "Görevde", 0, "Güvenlik Sorumlusu", 36, 36 },
                    { 37, "Görevde", 0, "Operatör", 37, 37 },
                    { 38, "Görevde", 0, "Teknik Destek", 38, 38 },
                    { 39, "Görevde", 0, "ISG Gözlemci", 39, 39 },
                    { 40, "Görevde", 0, "Vardiya Amiri", 40, 40 },
                    { 41, "Görevde", 0, "Güvenlik Sorumlusu", 41, 41 },
                    { 42, "Görevde", 0, "Operatör", 42, 42 },
                    { 43, "Görevde", 0, "Teknik Destek", 43, 43 },
                    { 44, "Görevde", 0, "ISG Gözlemci", 44, 44 },
                    { 45, "Görevde", 0, "Vardiya Amiri", 45, 45 },
                    { 46, "Görevde", 0, "Güvenlik Sorumlusu", 46, 46 },
                    { 47, "Görevde", 0, "Operatör", 47, 47 },
                    { 48, "Görevde", 0, "Teknik Destek", 48, 48 },
                    { 49, "Görevde", 0, "ISG Gözlemci", 49, 49 },
                    { 50, "Görevde", 0, "Vardiya Amiri", 50, 50 }
                });

            migrationBuilder.InsertData(
                table: "EkipmanRaporu",
                columns: new[] { "raporId", "arizaSayisi", "calismaSuresi", "ekipmanTuru" },
                values: new object[,]
                {
                    { 21, 0, 0, "Ağır İş Makinesi" },
                    { 22, 0, 0, "Ağır İş Makinesi" },
                    { 23, 0, 0, "Ağır İş Makinesi" },
                    { 24, 0, 0, "Ağır İş Makinesi" },
                    { 25, 0, 0, "Ağır İş Makinesi" },
                    { 26, 0, 0, "Ağır İş Makinesi" },
                    { 27, 0, 0, "Ağır İş Makinesi" },
                    { 28, 0, 0, "Ağır İş Makinesi" },
                    { 29, 0, 0, "Ağır İş Makinesi" },
                    { 30, 0, 0, "Ağır İş Makinesi" },
                    { 31, 0, 0, "Ağır İş Makinesi" },
                    { 32, 0, 0, "Ağır İş Makinesi" },
                    { 33, 0, 0, "Ağır İş Makinesi" },
                    { 34, 0, 0, "Ağır İş Makinesi" },
                    { 35, 0, 0, "Ağır İş Makinesi" },
                    { 36, 0, 0, "Ağır İş Makinesi" },
                    { 37, 0, 0, "Ağır İş Makinesi" },
                    { 38, 0, 0, "Ağır İş Makinesi" },
                    { 39, 0, 0, "Ağır İş Makinesi" },
                    { 40, 0, 0, "Ağır İş Makinesi" }
                });

            migrationBuilder.InsertData(
                table: "PersonelRaporu",
                columns: new[] { "raporId", "calismaSuresi", "personelSayisi", "uzmanlikAlani" },
                values: new object[,]
                {
                    { 1, 160m, 0, "Maden Mühendisliği" },
                    { 2, 160m, 0, "Maden Mühendisliği" },
                    { 3, 160m, 0, "Maden Mühendisliği" },
                    { 4, 160m, 0, "Maden Mühendisliği" },
                    { 5, 160m, 0, "Maden Mühendisliği" },
                    { 6, 160m, 0, "Maden Mühendisliği" },
                    { 7, 160m, 0, "Maden Mühendisliği" },
                    { 8, 160m, 0, "Maden Mühendisliği" },
                    { 9, 160m, 0, "Maden Mühendisliği" },
                    { 10, 160m, 0, "Maden Mühendisliği" },
                    { 11, 160m, 0, "Maden Mühendisliği" },
                    { 12, 160m, 0, "Maden Mühendisliği" },
                    { 13, 160m, 0, "Maden Mühendisliği" },
                    { 14, 160m, 0, "Maden Mühendisliği" },
                    { 15, 160m, 0, "Maden Mühendisliği" },
                    { 16, 160m, 0, "Maden Mühendisliği" },
                    { 17, 160m, 0, "Maden Mühendisliği" },
                    { 18, 160m, 0, "Maden Mühendisliği" },
                    { 19, 160m, 0, "Maden Mühendisliği" },
                    { 20, 160m, 0, "Maden Mühendisliği" }
                });

            migrationBuilder.InsertData(
                table: "SensorVerisi",
                columns: new[] { "sensorVerisiId", "birim", "deger", "ekipmanId", "olcumTarihi", "vardiyaId" },
                values: new object[,]
                {
                    { 1, "%", 21.5m, 501, new DateTime(2026, 1, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 2, "°C", 22.5m, 502, new DateTime(2026, 1, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 3, "%", 23.5m, 503, new DateTime(2026, 1, 1, 3, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 4, "°C", 24.5m, 504, new DateTime(2026, 1, 1, 4, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, "%", 25.5m, 505, new DateTime(2026, 1, 1, 5, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 6, "°C", 26.5m, 506, new DateTime(2026, 1, 1, 6, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 7, "%", 27.5m, 507, new DateTime(2026, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 8, "°C", 28.5m, 508, new DateTime(2026, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 9, "%", 29.5m, 509, new DateTime(2026, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 10, "°C", 30.5m, 510, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 11, "%", 31.5m, 511, new DateTime(2026, 1, 1, 11, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 12, "°C", 32.5m, 512, new DateTime(2026, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 13, "%", 33.5m, 513, new DateTime(2026, 1, 1, 13, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 14, "°C", 34.5m, 514, new DateTime(2026, 1, 1, 14, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 15, "%", 35.5m, 515, new DateTime(2026, 1, 1, 15, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 16, "°C", 36.5m, 516, new DateTime(2026, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 17, "%", 37.5m, 517, new DateTime(2026, 1, 1, 17, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 18, "°C", 38.5m, 518, new DateTime(2026, 1, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 19, "%", 39.5m, 519, new DateTime(2026, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 20, "°C", 40.5m, 520, new DateTime(2026, 1, 1, 20, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 21, "%", 41.5m, 521, new DateTime(2026, 1, 1, 21, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 22, "°C", 42.5m, 522, new DateTime(2026, 1, 1, 22, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 23, "%", 43.5m, 523, new DateTime(2026, 1, 1, 23, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 24, "°C", 44.5m, 524, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 25, "%", 45.5m, 525, new DateTime(2026, 1, 2, 1, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 26, "°C", 46.5m, 526, new DateTime(2026, 1, 2, 2, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 27, "%", 47.5m, 527, new DateTime(2026, 1, 2, 3, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 28, "°C", 48.5m, 528, new DateTime(2026, 1, 2, 4, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 29, "%", 49.5m, 529, new DateTime(2026, 1, 2, 5, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 30, "°C", 50.5m, 530, new DateTime(2026, 1, 2, 6, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 31, "%", 51.5m, 531, new DateTime(2026, 1, 2, 7, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 32, "°C", 52.5m, 532, new DateTime(2026, 1, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 33, "%", 53.5m, 533, new DateTime(2026, 1, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 34, "°C", 54.5m, 534, new DateTime(2026, 1, 2, 10, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 35, "%", 55.5m, 535, new DateTime(2026, 1, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 36, "°C", 56.5m, 536, new DateTime(2026, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 37, "%", 57.5m, 537, new DateTime(2026, 1, 2, 13, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 38, "°C", 58.5m, 538, new DateTime(2026, 1, 2, 14, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 39, "%", 59.5m, 539, new DateTime(2026, 1, 2, 15, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 40, "°C", 60.5m, 540, new DateTime(2026, 1, 2, 16, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 41, "%", 61.5m, 541, new DateTime(2026, 1, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 42, "°C", 62.5m, 542, new DateTime(2026, 1, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 43, "%", 63.5m, 543, new DateTime(2026, 1, 2, 19, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 44, "°C", 64.5m, 544, new DateTime(2026, 1, 2, 20, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 45, "%", 65.5m, 545, new DateTime(2026, 1, 2, 21, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 46, "°C", 66.5m, 546, new DateTime(2026, 1, 2, 22, 0, 0, 0, DateTimeKind.Unspecified), 7 },
                    { 47, "%", 67.5m, 547, new DateTime(2026, 1, 2, 23, 0, 0, 0, DateTimeKind.Unspecified), 8 },
                    { 48, "°C", 68.5m, 548, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 9 },
                    { 49, "%", 69.5m, 549, new DateTime(2026, 1, 3, 1, 0, 0, 0, DateTimeKind.Unspecified), 10 },
                    { 50, "°C", 70.5m, 550, new DateTime(2026, 1, 3, 2, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "VakaRaporu",
                columns: new[] { "raporId", "ciddiyetSeviyesi", "cozumSuresi", "personelId", "raporlayanEkipmanId" },
                values: new object[,]
                {
                    { 41, "Yüksek", 12.5m, 41, null },
                    { 42, "Yüksek", 12.5m, 42, null },
                    { 43, "Yüksek", 12.5m, 43, null },
                    { 44, "Yüksek", 12.5m, 44, null },
                    { 45, "Yüksek", 12.5m, 45, null },
                    { 46, "Yüksek", 12.5m, 46, null },
                    { 47, "Yüksek", 12.5m, 47, null },
                    { 48, "Yüksek", 12.5m, 48, null },
                    { 49, "Yüksek", 12.5m, 49, null },
                    { 50, "Yüksek", 12.5m, 50, null },
                    { 51, "Yüksek", 12.5m, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Aksiyon",
                columns: new[] { "mudahaleId", "ekipId", "mudahaleBaslangicSaati", "mudahaleBitisSaati", "mudahaleTuru", "uygulananCozum", "vakaId" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 12, 31, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 1 },
                    { 2, 3, new DateTime(2025, 12, 31, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 2 },
                    { 3, 4, new DateTime(2025, 12, 31, 21, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 3 },
                    { 4, 5, new DateTime(2025, 12, 31, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 4 },
                    { 5, 6, new DateTime(2025, 12, 31, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 5 },
                    { 6, 7, new DateTime(2025, 12, 31, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 6 },
                    { 7, 8, new DateTime(2025, 12, 31, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 7 },
                    { 8, 9, new DateTime(2025, 12, 31, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 8 },
                    { 9, 10, new DateTime(2025, 12, 31, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 9 },
                    { 10, 11, new DateTime(2025, 12, 31, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 10 },
                    { 11, 12, new DateTime(2025, 12, 31, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 11 },
                    { 12, 13, new DateTime(2025, 12, 31, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 12 },
                    { 13, 14, new DateTime(2025, 12, 31, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 13 },
                    { 14, 15, new DateTime(2025, 12, 31, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 14 },
                    { 15, 16, new DateTime(2025, 12, 31, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 15 },
                    { 16, 17, new DateTime(2025, 12, 31, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 16 },
                    { 17, 18, new DateTime(2025, 12, 31, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 17 },
                    { 18, 19, new DateTime(2025, 12, 31, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 18 },
                    { 19, 20, new DateTime(2025, 12, 31, 5, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 19 },
                    { 20, 21, new DateTime(2025, 12, 31, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 20 },
                    { 21, 22, new DateTime(2025, 12, 31, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 21 },
                    { 22, 23, new DateTime(2025, 12, 31, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 22 },
                    { 23, 24, new DateTime(2025, 12, 31, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 23 },
                    { 24, 25, new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 24 },
                    { 25, 26, new DateTime(2025, 12, 30, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 25 },
                    { 26, 27, new DateTime(2025, 12, 30, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 26 },
                    { 27, 28, new DateTime(2025, 12, 30, 21, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 27 },
                    { 28, 29, new DateTime(2025, 12, 30, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 28 },
                    { 29, 30, new DateTime(2025, 12, 30, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 29 },
                    { 30, 31, new DateTime(2025, 12, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 30 },
                    { 31, 32, new DateTime(2025, 12, 30, 17, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 31 },
                    { 32, 33, new DateTime(2025, 12, 30, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 32 },
                    { 33, 34, new DateTime(2025, 12, 30, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 33 },
                    { 34, 35, new DateTime(2025, 12, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 34 },
                    { 35, 36, new DateTime(2025, 12, 30, 13, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 35 },
                    { 36, 37, new DateTime(2025, 12, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 36 },
                    { 37, 38, new DateTime(2025, 12, 30, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 37 },
                    { 38, 39, new DateTime(2025, 12, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 38 },
                    { 39, 40, new DateTime(2025, 12, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 39 },
                    { 40, 41, new DateTime(2025, 12, 30, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 40 },
                    { 41, 42, new DateTime(2025, 12, 30, 7, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 41 },
                    { 42, 43, new DateTime(2025, 12, 30, 6, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 42 },
                    { 43, 44, new DateTime(2025, 12, 30, 5, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 43 },
                    { 44, 45, new DateTime(2025, 12, 30, 4, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 44 },
                    { 45, 46, new DateTime(2025, 12, 30, 3, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 45 },
                    { 46, 47, new DateTime(2025, 12, 30, 2, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 46 },
                    { 47, 48, new DateTime(2025, 12, 30, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Hata giderildi.", 47 },
                    { 48, 49, new DateTime(2025, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Parça değişimi yapıldı.", 48 },
                    { 49, 50, new DateTime(2025, 12, 29, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "Sistem resetlendi.", 49 },
                    { 50, 1, new DateTime(2025, 12, 29, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım", "ISG protokolü uygulandı.", 50 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcilDurumIletisim_personelId",
                table: "AcilDurumIletisim",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_Aksiyon_ekipId",
                table: "Aksiyon",
                column: "ekipId");

            migrationBuilder.CreateIndex(
                name: "IX_Aksiyon_vakaId",
                table: "Aksiyon",
                column: "vakaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ekip_personelId",
                table: "Ekip",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_Ekip_vardiyaId",
                table: "Ekip",
                column: "vardiyaId");

            migrationBuilder.CreateIndex(
                name: "IX_LogKaydi_ekipmanId",
                table: "LogKaydi",
                column: "ekipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_LogKaydi_personelId",
                table: "LogKaydi",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rapor_ekipmanId",
                table: "Rapor",
                column: "ekipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Rapor_personelId",
                table: "Rapor",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorVerisi_ekipmanId",
                table: "SensorVerisi",
                column: "ekipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorVerisi_vardiyaId",
                table: "SensorVerisi",
                column: "vardiyaId");

            migrationBuilder.CreateIndex(
                name: "IX_TakipCihazi_ekipmanId",
                table: "TakipCihazi",
                column: "ekipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_TakipCihazi_personelId",
                table: "TakipCihazi",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaka_ilgiliEkipmanId",
                table: "Vaka",
                column: "ilgiliEkipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaka_personelId",
                table: "Vaka",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaka_raporlayanEkipmanId",
                table: "Vaka",
                column: "raporlayanEkipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_VakaRaporu_personelId",
                table: "VakaRaporu",
                column: "personelId");

            migrationBuilder.CreateIndex(
                name: "IX_VakaRaporu_raporlayanEkipmanId",
                table: "VakaRaporu",
                column: "raporlayanEkipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Vardiya_ekipmanId",
                table: "Vardiya",
                column: "ekipmanId");

            migrationBuilder.CreateIndex(
                name: "IX_Vardiya_vardiyaIsgSorumlusu",
                table: "Vardiya",
                column: "vardiyaIsgSorumlusu");

            migrationBuilder.CreateIndex(
                name: "IX_Vardiya_vardiyaSorumlusu",
                table: "Vardiya",
                column: "vardiyaSorumlusu");

            migrationBuilder.CreateIndex(
                name: "IX_Vardiya_vardiyaTeknikSorumlusu",
                table: "Vardiya",
                column: "vardiyaTeknikSorumlusu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcilDurumIletisim");

            migrationBuilder.DropTable(
                name: "Aksiyon");

            migrationBuilder.DropTable(
                name: "EkipmanRaporu");

            migrationBuilder.DropTable(
                name: "Ekskavator");

            migrationBuilder.DropTable(
                name: "ElAletleri");

            migrationBuilder.DropTable(
                name: "Hafriyat");

            migrationBuilder.DropTable(
                name: "Kepce");

            migrationBuilder.DropTable(
                name: "Kirici");

            migrationBuilder.DropTable(
                name: "LogKaydi");

            migrationBuilder.DropTable(
                name: "PersonelRaporu");

            migrationBuilder.DropTable(
                name: "SaglikBilgileri");

            migrationBuilder.DropTable(
                name: "SensorVerisi");

            migrationBuilder.DropTable(
                name: "TakipCihazi");

            migrationBuilder.DropTable(
                name: "VakaRaporu");

            migrationBuilder.DropTable(
                name: "Ekip");

            migrationBuilder.DropTable(
                name: "Vaka");

            migrationBuilder.DropTable(
                name: "Sensor");

            migrationBuilder.DropTable(
                name: "Rapor");

            migrationBuilder.DropTable(
                name: "Vardiya");

            migrationBuilder.DropTable(
                name: "Ekipman");

            migrationBuilder.DropTable(
                name: "Personeller");
        }
    }
}
