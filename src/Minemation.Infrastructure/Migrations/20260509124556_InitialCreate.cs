using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minemation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
