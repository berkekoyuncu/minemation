using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minemation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ekipman",
                columns: new[] { "ekipmanId", "RFIDEtiket", "agirlik", "bakimFormu", "boyut", "durum", "ekipmanAdi", "ekipmanMarka", "ekipmanModel", "garantiBaslangicTarihi", "garantiBelgesi", "gelecekBakimTarihi", "kullanimKilavuzu", "operasyonTuru", "satinAlmaBelgesi", "satinAlmaTarihi", "seriNo", "sonBakimTarihi", "tedarikciFirma", "teknikDokuman", "ureticiFirma", "uretimYili" },
                values: new object[] { 101, "TAG-EQ-101", 94000.00m, "/docs/service.pdf", 12.50m, "Aktif", "Dev Ekskavatör CAT-395", "Caterpillar", "395-L", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/warranty.pdf", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/manual.pdf", "Kazı ve Yükleme", "/docs/invoice.pdf", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-CAT-999", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Borusan Makina", "/docs/cat395.pdf", "CAT Global", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Personeller",
                columns: new[] { "personelId", "adres", "calisanTipi", "calismaKonumu", "cinsiyet", "departman", "dogumTarihi", "eposta", "ikinciTelNo", "iseGirisTarihi", "kullaniciRolu", "personelAdi", "personelDurumu", "personelSoyadi", "rfidKartNumarasi", "sifreHash", "sonGirisTarihi", "tckn", "telNo", "uzmanlik" },
                values: new object[] { 1, "Ankara", "Tam Zamanlı", "Merkez Sahası", "Kadın", "Yönetim", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "busra@minemation.com", "05551112234", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "Büşra", "Aktif", "Arslan", "RFID-001", "AQAAAAEAACcQAAAAE...", new DateTime(2026, 5, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "12345678901", "05551112233", "Mühendis" });

            migrationBuilder.InsertData(
                table: "Ekskavator",
                columns: new[] { "ekipmanId", "bomUzunlugu", "kovaKapasitesi", "maksimumKaziDerinligi", "motorGucu", "paletTipi", "plaka" },
                values: new object[] { 101, 7.2m, 6.5m, 9.7m, 543m, "Çelik Ağır Hizmet", "06 MINE 01" });

            migrationBuilder.InsertData(
                table: "SaglikBilgileri",
                columns: new[] { "personelId", "acilDurumNotu", "alerjiler", "kanGrubu", "kronikHastaliklar", "saglikCalismaKisitlamalari", "saglikDurumu", "sonMuayeneTarihi" },
                values: new object[] { 1, "İlaç alerjisi yok.", "[\"Polen\"]", "A Rh+", "[]", "Yok", "Genel sağlık durumu iyi.", new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Sensor",
                columns: new[] { "ekipmanId", "baglantiProtokolu", "haberlesmeTipi", "hassasiyet", "maxEsikDeger", "minEsikDeger", "sensorDurumu", "sensorTipi" },
                values: new object[] { 101, "MQTT", "Kablosuz 4G", 0.5, 110.0, 20.0, "Aktif", "Sıcaklık ve Basınç" });

            migrationBuilder.InsertData(
                table: "Vaka",
                columns: new[] { "vakaId", "ilgiliEkipmanId", "olayNedeni", "personelId", "raporlayanEkipmanId", "vakaAciklamasi", "vakaAdi", "vakaCiddiyetSeviyesi", "vakaDurumu", "vakaKapanmaTarihi", "vakaOlusmaTarihi", "vakaTuru" },
                values: new object[] { 1, 101, "Soğutma sıvısı sızıntısı şüphesi", 1, null, "Sensör verisi normalin üzerine çıktı.", "Yüksek Motor Sıcaklığı", "Kritik", "İncelemede", null, new DateTime(2026, 5, 9, 9, 15, 0, 0, DateTimeKind.Unspecified), "Teknik Arıza" });

            migrationBuilder.InsertData(
                table: "Vardiya",
                columns: new[] { "vardiyaId", "calismaBolgesi", "ekipSayisi", "ekipmanId", "ekipmanOperatoru", "ekipmanSayisi", "operasyonRiskSeviyesi", "operasyonTipi", "personelSayisi", "toplaVardiyaSuresi", "vardiyaAdi", "vardiyaBaslangicTarihi", "vardiyaBitisTarihi", "vardiyaDurumu", "vardiyaIsgSorumlusu", "vardiyaNotlari", "vardiyaOlusturmaTarihi", "vardiyaSorumlusu", "vardiyaSupervizoru", "vardiyaTanimi", "vardiyaTeknikSorumlusu", "vardiyaTipi" },
                values: new object[] { 1, "Batı Ocağı", 1, null, "Ahmet Operatör", 2, "Orta", "Dekapaj", 5, 8, "Gündüz Vardiyası A", new DateTime(2026, 5, 9, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 16, 0, 0, 0, DateTimeKind.Unspecified), "Aktif", 1, "Zemin kaygan, dikkatli olunmalı.", new DateTime(2026, 5, 9, 7, 30, 0, 0, DateTimeKind.Unspecified), 1, "Büşra Arslan", "08:00 - 16:00 Kazı Operasyonu", 1, "Üretim" });

            migrationBuilder.InsertData(
                table: "Ekip",
                columns: new[] { "ekipId", "durum", "ekipUyeSayisi", "personelGorevi", "personelId", "vardiyaId" },
                values: new object[] { 1, "Görevde", 1, "Vardiya Amiri", 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ekip",
                keyColumn: "ekipId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ekskavator",
                keyColumn: "ekipmanId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "SaglikBilgileri",
                keyColumn: "personelId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sensor",
                keyColumn: "ekipmanId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Vaka",
                keyColumn: "vakaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ekipman",
                keyColumn: "ekipmanId",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Vardiya",
                keyColumn: "vardiyaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Personeller",
                keyColumn: "personelId",
                keyValue: 1);
        }
    }
}
