using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Minemation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VeriTohumlamaSabitlendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AcilDurumIletisim",
                columns: new[] { "acilDurumKisisiId", "acilDurumKisileriAd", "acilDurumKisileriSoyad", "acilDurumKisileriTelNo", "acilDurumKisileriYakinlik", "personelId" },
                values: new object[] { 1, "Mehmet", "Arslan", "05320000001", "Baba", 1 });

            migrationBuilder.InsertData(
                table: "Aksiyon",
                columns: new[] { "mudahaleId", "ekipId", "mudahaleBaslangicSaati", "mudahaleBitisSaati", "mudahaleTuru", "uygulananCozum", "vakaId" },
                values: new object[] { 1, 1, new DateTime(2026, 5, 9, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 10, 15, 0, 0, DateTimeKind.Unspecified), "Teknik Müdahale", "Motor soğutma fanı kontrol edildi ve temizlendi.", 1 });

            migrationBuilder.InsertData(
                table: "Ekipman",
                columns: new[] { "ekipmanId", "RFIDEtiket", "agirlik", "bakimFormu", "boyut", "durum", "ekipmanAdi", "ekipmanMarka", "ekipmanModel", "garantiBaslangicTarihi", "garantiBelgesi", "gelecekBakimTarihi", "kullanimKilavuzu", "operasyonTuru", "satinAlmaBelgesi", "satinAlmaTarihi", "seriNo", "sonBakimTarihi", "tedarikciFirma", "teknikDokuman", "ureticiFirma", "uretimYili" },
                values: new object[,]
                {
                    { 102, "TAG-TRUCK-102", 0m, "/docs/volvo_service.pdf", 0m, "Aktif", "Volvo A60H Kamyon", "Volvo", "A60H", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/volvo_warranty.pdf", new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/volvo_manual.pdf", "Taşıma", "/docs/volvo_invoice.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-VOL-001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ascendum Makina", "/docs/volvo_a60h.pdf", "Volvo CE", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 103, "TAG-LOADER-103", 0m, "/docs/komatsu_service.pdf", 0m, "Aktif", "Komatsu WA600", "Komatsu", "WA600", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/komatsu_warranty.pdf", new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "/docs/komatsu_manual.pdf", "Yükleme", "/docs/komatsu_invoice.pdf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SN-KOM-002", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Temsa İş Makinaları", "/docs/komatsu_wa600.pdf", "Komatsu Ltd.", new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "LogKaydi",
                columns: new[] { "logKaydiID", "durum", "ekipmanId", "ipAdresi", "islemTipi", "logKaydiAciklamasi", "logKaydiTarihi", "onemSeviyesi", "personelId" },
                values: new object[] { 1, "Aktif", null, "192.168.1.50", "Sistem Girişi", "Büşra Arslan sisteme başarılı bir şekilde giriş yaptı.", new DateTime(2026, 5, 9, 10, 0, 0, 0, DateTimeKind.Unspecified), "Bilgi", 1 });

            migrationBuilder.InsertData(
                table: "Rapor",
                columns: new[] { "raporId", "ekipmanId", "personelId", "raporAciklamasi", "raporAdi", "raporDosyaYolu", "raporOlusturmaTarihi", "raporTuru", "zamanAraligi" },
                values: new object[,]
                {
                    { 500, null, 1, "Tüm ağır makinelerin genel durum raporu.", "Mayıs 2026 Aylık Bakım Özeti", "/reports/mayis_bakim.pdf", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bakım Raporu", "Mayıs 2026" },
                    { 501, null, 1, "Personel çalışma süreleri ve uzmanlık dağılım analizi.", "İK Performans Analizi", "/reports/ik_performans.pdf", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "İnsan Kaynakları", "Nisan-Mayıs 2026" }
                });

            migrationBuilder.InsertData(
                table: "SensorVerisi",
                columns: new[] { "sensorVerisiId", "birim", "deger", "ekipmanId", "olcumTarihi", "vardiyaId" },
                values: new object[] { 1, "°C", 85.5m, 101, new DateTime(2026, 5, 9, 9, 10, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "TakipCihazi",
                columns: new[] { "takipCihaziId", "ekipmanId", "personelId", "pilSeviyesi", "takipCihaziDurumu", "takipCihaziHaberlesmeProtokolu", "takipCihaziModeli", "takipCihaziSeriNo", "takipCihaziSonBaglantiZamani", "takipCihaziTuru" },
                values: new object[] { 1, null, 1, 92.5m, "Aktif", "LoRaWAN", "MineTrack-V2", "GPS-MINE-555", new DateTime(2026, 5, 11, 14, 0, 0, 0, DateTimeKind.Unspecified), "Kişisel Takip" });

            migrationBuilder.InsertData(
                table: "EkipmanRaporu",
                columns: new[] { "raporId", "arizaSayisi", "calismaSuresi", "ekipmanTuru" },
                values: new object[] { 500, 2, 160, "Ekskavatör" });

            migrationBuilder.InsertData(
                table: "PersonelRaporu",
                columns: new[] { "raporId", "calismaSuresi", "personelSayisi", "uzmanlikAlani" },
                values: new object[] { 501, 2100.50m, 12, "Mühendislik" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AcilDurumIletisim",
                keyColumn: "acilDurumKisisiId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Aksiyon",
                keyColumn: "mudahaleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ekipman",
                keyColumn: "ekipmanId",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Ekipman",
                keyColumn: "ekipmanId",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "EkipmanRaporu",
                keyColumn: "raporId",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "LogKaydi",
                keyColumn: "logKaydiID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PersonelRaporu",
                keyColumn: "raporId",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "SensorVerisi",
                keyColumn: "sensorVerisiId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TakipCihazi",
                keyColumn: "takipCihaziId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rapor",
                keyColumn: "raporId",
                keyValue: 500);

            migrationBuilder.DeleteData(
                table: "Rapor",
                keyColumn: "raporId",
                keyValue: 501);
        }
    }
}
