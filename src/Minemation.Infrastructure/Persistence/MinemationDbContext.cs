using Microsoft.EntityFrameworkCore;
using Minemation.Domain.Entities;

namespace Minemation.Infrastructure.Persistence;

public class MinemationDbContext : DbContext
{
    public MinemationDbContext(DbContextOptions<MinemationDbContext> options)
        : base(options)
    {
    }

    // Personel
    public DbSet<Personel> Personeller { get; set; }
    public DbSet<SaglikBilgileri> SaglikBilgileri { get; set; }
    public DbSet<AcilDurumIletisim> AcilDurumIletisim { get; set; }

    // Ekipman
    public DbSet<Ekipman> Ekipmanlar { get; set; }
    public DbSet<Ekskavator> Ekskavatorler { get; set; }
    public DbSet<Kepce> Kepceler { get; set; }
    public DbSet<Kirici> Kiricilar { get; set; }
    public DbSet<Hafriyat> Hafriyatlar { get; set; }
    public DbSet<ElAletleri> ElAletleri { get; set; }

    // Sensor / Risk İzleme
    public DbSet<Sensor> Sensorler { get; set; }
    public DbSet<SensorVerisi> SensorVerileri { get; set; }

    // Harita / Takip
    public DbSet<TakipCihazi> TakipCihazlari { get; set; }

    // Vardiya / Ekip
    public DbSet<Vardiya> Vardiyalar { get; set; }
    public DbSet<Ekip> Ekipler { get; set; }

    // Vaka / Aksiyon
    public DbSet<Vaka> Vakalar { get; set; }
    public DbSet<Aksiyon> Aksiyonlar { get; set; }

    // Rapor
    public DbSet<Rapor> Raporlar { get; set; }
    public DbSet<PersonelRaporu> PersonelRaporlari { get; set; }
    public DbSet<EkipmanRaporu> EkipmanRaporlari { get; set; }
    public DbSet<VakaRaporu> VakaRaporlari { get; set; }

    // Log
    public DbSet<LogKaydi> LogKayitlari { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PERSONEL
        modelBuilder.Entity<Personel>(entity =>
        {
            entity.HasKey(e => e.personelId);

            entity.Property(e => e.personelAdi)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.personelSoyadi)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.uzmanlik)
                .HasMaxLength(100);

            entity.Property(e => e.tckn)
                .HasColumnType("char(11)")
                .IsRequired();

            entity.Property(e => e.personelDurumu)
                .HasMaxLength(50);

            entity.Property(e => e.cinsiyet)
                .HasMaxLength(20);

            entity.Property(e => e.telNo)
                .HasMaxLength(20);

            entity.Property(e => e.ikinciTelNo)
                .HasMaxLength(20);

            entity.Property(e => e.eposta)
                .HasMaxLength(100);

            entity.Property(e => e.calisanTipi)
                .HasMaxLength(50);

            entity.Property(e => e.rfidKartNumarasi)
                .HasMaxLength(50);

            entity.Property(e => e.kullaniciRolu)
                .HasMaxLength(50);

            entity.Property(e => e.departman)
                .HasMaxLength(100);

            entity.Property(e => e.calismaKonumu)
                .HasMaxLength(100);

            entity.Property(e => e.sifreHash)
                .HasMaxLength(255);

            entity.Ignore(e => e.calisabildigiEkipmaTurleri);
        });

        // SAĞLIK BİLGİLERİ
        modelBuilder.Entity<SaglikBilgileri>(entity =>
        {
            entity.HasKey(e => e.personelId);

            entity.Property(e => e.kanGrubu)
                .HasMaxLength(5);

            entity.Property(e => e.saglikDurumu)
                .HasColumnType("text");

            entity.Property(e => e.saglikCalismaKisitlamalari)
                .HasColumnType("text");

            entity.Property(e => e.acilDurumNotu)
                .HasColumnType("text");

            entity.Ignore(e => e.kronikHastaliklar);
            entity.Ignore(e => e.alerjiler);

            entity.HasOne(e => e.Personel)
                .WithOne(e => e.SaglikBilgileri)
                .HasForeignKey<SaglikBilgileri>(e => e.personelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ACİL DURUM İLETİŞİM
        modelBuilder.Entity<AcilDurumIletisim>(entity =>
        {
            entity.HasKey(e => e.acilDurumKisisiId);

            entity.Property(e => e.acilDurumKisileriAd)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.acilDurumKisileriSoyad)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.acilDurumKisileriYakinlik)
                .HasMaxLength(50);

            entity.Property(e => e.acilDurumKisileriTelNo)
                .HasMaxLength(20)
                .IsRequired();

            entity.HasOne(e => e.Personel)
                .WithMany(e => e.AcilDurumKisileri)
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // EKİPMAN
        modelBuilder.Entity<Ekipman>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.ekipmanAdi)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.ekipmanMarka)
                .HasMaxLength(100);

            entity.Property(e => e.ekipmanModel)
                .HasMaxLength(100);

            entity.Property(e => e.durum)
                .HasMaxLength(50);

            entity.Property(e => e.seriNo)
                .HasMaxLength(100);

            entity.Property(e => e.RFIDEtiket)
                .HasMaxLength(50);

            entity.Property(e => e.ureticiFirma)
                .HasMaxLength(100);

            entity.Property(e => e.tedarikciFirma)
                .HasMaxLength(100);

            entity.Property(e => e.teknikDokuman)
                .HasMaxLength(255);

            entity.Property(e => e.kullanimKilavuzu)
                .HasMaxLength(255);

            entity.Property(e => e.garantiBelgesi)
                .HasMaxLength(255);

            entity.Property(e => e.bakimFormu)
                .HasMaxLength(255);

            entity.Property(e => e.satinAlmaBelgesi)
                .HasMaxLength(255);

            entity.Property(e => e.operasyonTuru)
                .HasMaxLength(100);
        });

        // EKSKAVATOR
        modelBuilder.Entity<Ekskavator>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.plaka)
                .HasMaxLength(20);

            entity.Property(e => e.paletTipi)
                .HasMaxLength(50);

            entity.HasOne(e => e.Ekipman)
                .WithOne()
                .HasForeignKey<Ekskavator>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // KEPÇE
        modelBuilder.Entity<Kepce>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.plaka)
                .HasMaxLength(20);

            entity.HasOne(e => e.Ekipman)
                .WithOne()
                .HasForeignKey<Kepce>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // KIRICI
        modelBuilder.Entity<Kirici>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.ucTipi)
                .HasMaxLength(50);

            entity.Property(e => e.gerekenYagDebisi)
                .HasMaxLength(50);

            entity.HasOne(e => e.Ekipman)
                .WithOne()
                .HasForeignKey<Kirici>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // HAFRİYAT
        modelBuilder.Entity<Hafriyat>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.plaka)
                .HasMaxLength(20);

            entity.HasOne(e => e.Ekipman)
                .WithOne()
                .HasForeignKey<Hafriyat>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // EL ALETLERİ
        modelBuilder.Entity<ElAletleri>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.gucKaynagiTipi)
                .HasMaxLength(50);

            entity.Property(e => e.kullanimAmaci)
                .HasMaxLength(100);

            entity.HasOne(e => e.Ekipman)
                .WithOne()
                .HasForeignKey<ElAletleri>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SENSOR
        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.ekipmanId);

            entity.Property(e => e.sensorTipi)
                .HasMaxLength(50);

            entity.Property(e => e.sensorDurumu)
                .HasMaxLength(50);

            entity.Property(e => e.baglantiProtokolu)
                .HasMaxLength(50);

            entity.Property(e => e.haberlesmeTipi)
                .HasMaxLength(50);

            entity.HasOne(e => e.Ekipman)
                .WithOne(e => e.Sensor)
                .HasForeignKey<Sensor>(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SENSOR VERİSİ
        modelBuilder.Entity<SensorVerisi>(entity =>
        {
            entity.HasKey(e => e.sensorVerisiId);

            entity.Property(e => e.birim)
                .HasMaxLength(20);

            // Sensor entity'sinde sensorId olmadığı için sensorId veritabanına alınmıyor.
            entity.Ignore(e => e.sensorId);

            // Aynı ekipmanId hem Sensor hem Ekipman ilişkisi için kullanılmasın diye
            // doğrudan Ekipman navigation'ı şimdilik ignore ediliyor.
            entity.Ignore(e => e.Ekipman);

            entity.HasOne(e => e.Sensor)
                .WithMany()
                .HasForeignKey(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vardiya)
                .WithMany()
                .HasForeignKey(e => e.vardiyaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TAKİP CİHAZI
        modelBuilder.Entity<TakipCihazi>(entity =>
        {
            entity.HasKey(e => e.takipCihaziId);

            entity.Property(e => e.takipCihaziSeriNo)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.takipCihaziTuru)
                .HasMaxLength(50);

            entity.Property(e => e.takipCihaziModeli)
                .HasMaxLength(100);

            entity.Property(e => e.takipCihaziDurumu)
                .HasMaxLength(50);

            entity.Property(e => e.takipCihaziHaberlesmeProtokolu)
                .HasMaxLength(50);

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Ekipman)
                .WithMany()
                .HasForeignKey(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // VARDİYA
        modelBuilder.Entity<Vardiya>(entity =>
        {
            entity.HasKey(e => e.vardiyaId);

            entity.Property(e => e.vardiyaAdi)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.vardiyaTanimi)
                .HasColumnType("text");

            entity.Property(e => e.vardiyaSupervizoru)
                .HasMaxLength(100);

            entity.Property(e => e.vardiyaDurumu)
                .HasMaxLength(50);

            entity.Property(e => e.vardiyaTipi)
                .HasMaxLength(50);

            entity.Property(e => e.calismaBolgesi)
                .HasMaxLength(100);

            entity.Property(e => e.operasyonTipi)
                .HasMaxLength(100);

            entity.Property(e => e.operasyonRiskSeviyesi)
                .HasMaxLength(50);

            entity.Property(e => e.vardiyaNotlari)
                .HasColumnType("text");

            entity.Property(e => e.ekipmanOperatoru)
                .HasMaxLength(100);

            entity.HasOne(e => e.VardiyaSorumlusuPersonel)
                .WithMany()
                .HasForeignKey(e => e.vardiyaSorumlusu)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.IsgSorumlusuPersonel)
                .WithMany()
                .HasForeignKey(e => e.vardiyaIsgSorumlusu)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.TeknikSorumlusuPersonel)
                .WithMany()
                .HasForeignKey(e => e.vardiyaTeknikSorumlusu)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // EKİP
        modelBuilder.Entity<Ekip>(entity =>
        {
            entity.HasKey(e => e.ekipId);

            entity.Property(e => e.personelGorevi)
                .HasMaxLength(50);

            entity.Property(e => e.durum)
                .HasMaxLength(50);

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Vardiya)
                .WithMany()
                .HasForeignKey(e => e.vardiyaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // VAKA
        modelBuilder.Entity<Vaka>(entity =>
        {
            entity.HasKey(e => e.vakaId);

            entity.Property(e => e.vakaAdi)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.vakaTuru)
                .HasMaxLength(50);

            entity.Property(e => e.vakaCiddiyetSeviyesi)
                .HasMaxLength(20);

            entity.Property(e => e.vakaDurumu)
                .HasMaxLength(20);

            entity.Property(e => e.vakaAciklamasi)
                .HasColumnType("text");

            entity.Property(e => e.olayNedeni)
                .HasMaxLength(200);

            entity.Property(e => e.vakaOlusmaTarihi)
                .HasColumnType("datetime");

            entity.Property(e => e.vakaKapanmaTarihi)
                .HasColumnType("datetime");

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Ekipman)
                .WithMany()
                .HasForeignKey(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AKSİYON
        modelBuilder.Entity<Aksiyon>(entity =>
        {
            entity.HasKey(e => e.mudahaleId);

            entity.Property(e => e.mudahaleTuru)
                .HasMaxLength(50);

            entity.Property(e => e.uygulananCozum)
                .HasColumnType("text");

            entity.Property(e => e.mudahaleBaslangicSaati)
                .IsRequired();

            entity.HasOne(e => e.Vaka)
                .WithMany()
                .HasForeignKey(e => e.vakaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Ekip)
                .WithMany()
                .HasForeignKey(e => e.ekipId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // RAPOR
        modelBuilder.Entity<Rapor>(entity =>
        {
            entity.HasKey(e => e.raporId);

            entity.Property(e => e.raporAdi)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.raporTuru)
                .HasMaxLength(50);

            entity.Property(e => e.raporAciklamasi)
                .HasColumnType("text");

            entity.Property(e => e.raporDosyaYolu)
                .HasMaxLength(255);

            entity.Property(e => e.zamanAraligi)
                .HasMaxLength(50);

            entity.Property(e => e.raporOlusturmaTarihi)
                .IsRequired();

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // PERSONEL RAPORU
        modelBuilder.Entity<PersonelRaporu>(entity =>
        {
            entity.HasKey(e => e.raporId);

            entity.Property(e => e.uzmanlikAlani)
                .HasMaxLength(100);

            entity.Property(e => e.calismaSuresi)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Rapor)
                .WithOne()
                .HasForeignKey<PersonelRaporu>(e => e.raporId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // EKİPMAN RAPORU
        modelBuilder.Entity<EkipmanRaporu>(entity =>
        {
            entity.HasKey(e => e.raporId);

            entity.Property(e => e.ekipmanTuru)
                .HasMaxLength(100);

            entity.HasOne(e => e.Rapor)
                .WithOne()
                .HasForeignKey<EkipmanRaporu>(e => e.raporId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // VAKA RAPORU
        modelBuilder.Entity<VakaRaporu>(entity =>
        {
            entity.HasKey(e => e.raporId);

            entity.Property(e => e.ciddiyetSeviyesi)
                .HasMaxLength(50);

            entity.Property(e => e.cozumSuresi)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Rapor)
                .WithOne()
                .HasForeignKey<VakaRaporu>(e => e.raporId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // LOG KAYDI
        modelBuilder.Entity<LogKaydi>(entity =>
        {
            entity.HasKey(e => e.logKaydiID);

            entity.Property(e => e.islemTipi)
                .HasMaxLength(100);

            entity.Property(e => e.logKaydiAciklamasi)
                .HasColumnType("text");

            entity.Property(e => e.ipAdresi)
                .HasMaxLength(50);

            entity.Property(e => e.önemSeviyesi)
                .HasMaxLength(50);

            entity.Property(e => e.durum)
                .HasMaxLength(50);

            entity.Property(e => e.logKaydiTarihi)
                .IsRequired();

            entity.HasOne(e => e.Personel)
                .WithMany()
                .HasForeignKey(e => e.personelId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Ekipman)
                .WithMany()
                .HasForeignKey(e => e.ekipmanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}