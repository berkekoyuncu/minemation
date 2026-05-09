using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Minemation.Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Minemation.Infrastructure.Persistence
{
    public class MinemationDbContext : DbContext
    {
        public MinemationDbContext(DbContextOptions<MinemationDbContext> options) : base(options) { }

        // Domain'de oluşturulan tüm sınıfların DbSet olarak eklenmesi
        public DbSet<Personel> Personeller { get; set; }
        public DbSet<SaglikBilgileri> SaglikBilgileri { get; set; }
        public DbSet<AcilDurumIletisim> AcilDurumIletisim { get; set; }
        public DbSet<Ekip> Ekip { get; set; }
        public DbSet<Ekipman> Ekipman { get; set; }
        public DbSet<Ekskavator> Ekskavator { get; set; }
        public DbSet<Kepce> Kepce { get; set; }
        public DbSet<ElAletleri> ElAletleri { get; set; }
        public DbSet<Hafriyat> Hafriyat { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<SensorVerisi> SensorVerisi { get; set; }
        public DbSet<Kirici> Kirici { get; set; }
        public DbSet<Vardiya> Vardiya { get; set; }
        public DbSet<TakipCihazi> TakipCihazi { get; set; }
        public DbSet<LogKaydi> LogKaydi { get; set; }
        public DbSet<Vaka> Vaka { get; set; }
        public DbSet<Aksiyon> Aksiyon { get; set; }
        public DbSet<Rapor> Rapor { get; set; }
        public DbSet<PersonelRaporu> PersonelRaporu { get; set; }
        public DbSet<EkipmanRaporu> EkipmanRaporu { get; set; }
        public DbSet<VakaRaporu> VakaRaporu { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. ÇEKİRDEK MODÜL (PERSONEL & ORGANİZASYON) ---

            // --- PERSONEL YAPILANDIRMASI ---
            modelBuilder.Entity<Personel>(entity =>
            {
                entity.HasKey(e => e.personelId);
                entity.Property(e => e.tckn).HasColumnType("char(11)").IsRequired();
                entity.Property(e => e.rfidKartNumarasi).HasMaxLength(50);

                // List<string> tipi veritabanında doğrudan saklanamaz. 
                // Ya bunu string'e çevirmelisin ya da EF Core'a nasıl saklayacağını söylemelisin.
                // Şimdilik hata almamak için ignore ediyoruz veya string olarak eşliyoruz.
                entity.Ignore(e => e.calisabildigiEkipmaTurleri);
            });


            // Sağlık Bilgileri Yapılandırması
            modelBuilder.Entity<SaglikBilgileri>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama (Shared Primary Key)
                entity.HasKey(e => e.personelId);

                // 2. Metin Alanı Kısıtlamaları
                entity.Property(e => e.kanGrubu)
                      .HasMaxLength(5); // A Rh+, AB- vb.

                entity.Property(e => e.saglikDurumu)
                      .HasColumnType("text");

                // 3. List<string> Dönüşümleri (Value Conversion)
                // SQL listeyi anlamaz, bu yüzden JSON string olarak saklıyoruz.
                entity.Property(e => e.kronikHastaliklar)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
                      .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                          (c1, c2) => c1.SequenceEqual(c2),
                          c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                          c => c.ToList()));

                entity.Property(e => e.alerjiler)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                          v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
                      .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                          (c1, c2) => c1.SequenceEqual(c2),
                          c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                          c => c.ToList()));

                // 4. İlişki Tanımı: Bire-Bir (1:1)
                entity.HasOne(s => s.Personel)
                      .WithOne(p => p.SaglikBilgileri)
                      .HasForeignKey<SaglikBilgileri>(s => s.personelId)
                      .OnDelete(DeleteBehavior.Cascade); // Personel silinirse sağlık bilgisi de silinir
            });




            // Acil Durum İletişim Yapılandırması
            modelBuilder.Entity<AcilDurumIletisim>(entity =>
            {
                    // 1. Birincil Anahtar (Kendi benzersiz Id'si)
                    entity.HasKey(e => e.acilDurumKisisiId);

                    // 2. Özellik Kısıtlamaları (İsimlendirmelerine göre)
                    entity.Property(e => e.acilDurumKisileriAd)
                      .HasMaxLength(50)
                      .IsRequired();

                    entity.Property(e => e.acilDurumKisileriSoyad)
                      .HasMaxLength(50)
                      .IsRequired();



                    entity.Property(e => e.acilDurumKisileriTelNo)
                      .HasMaxLength(20)
                      .IsRequired();

                    entity.Property(e => e.acilDurumKisileriYakinlik)
                      .HasMaxLength(50);

                    // 3. İlişki Tanımı (Bire-Çok / 1:N)
                    // Bir personelId'ye bağlı birden fazla acil durum kişisi olabilir.
                    entity.HasOne(d => d.Personel)
                      .WithMany(p => p.AcilDurumKisileri) // Personel.cs içindeki koleksiyon ismi
                      .HasForeignKey(d => d.personelId)
                      .OnDelete(DeleteBehavior.Cascade); // Personel silinince iletişim kayıtları da silinir.
            });



            // --- 2. EKİPMAN MODÜLÜ ---

            // Ekipman Yapılandırması
            modelBuilder.Entity<Ekipman>(entity =>
            {
                // 1. Birincil Anahtar
                entity.HasKey(e => e.ekipmanId);

                // 2. Metin Alanı Kısıtlamaları
                entity.Property(e => e.ekipmanAdi)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.ekipmanMarka)
                      .HasMaxLength(50);

                entity.Property(e => e.ekipmanModel)
                      .HasMaxLength(50);

                entity.Property(e => e.seriNo)
                      .HasMaxLength(50);

                entity.Property(e => e.RFIDEtiket)
                      .HasMaxLength(50);

                // 3. Sayısal Veri Hassasiyeti (Decimal alanlar için önemlidir)
                entity.Property(e => e.boyut)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.agirlik)
                      .HasColumnType("decimal(18,2)");

                // 4. Dosya Yolları (Dokümanlar için uzunluk sınırı)
                // PDF, Image gibi dosyalar sunucuda saklanacağı için dosya yolu (Path) uzun tutulur.
                entity.Property(e => e.teknikDokuman)
                      .HasMaxLength(500) // Sunucudaki klasör derinliği artabileceği için 500 daha güvenlidir
                      .IsUnicode(true);  // Özel karakterli dosya isimlerini (Türkçe karakter vb.) desteklemesi için

                entity.Property(e => e.kullanimKilavuzu)
                      .HasMaxLength(500)
                      .IsUnicode(true);

                entity.Property(e => e.garantiBelgesi)
                      .HasMaxLength(500)
                      .IsUnicode(true);

                entity.Property(e => e.bakimFormu)
                      .HasMaxLength(500)
                      .IsUnicode(true);

                entity.Property(e => e.satinAlmaBelgesi)
                      .HasMaxLength(500)
                      .IsUnicode(true);

                // 5. İlişki Tanımı: Sensör (1:1 İlişki)
                // Ekipman'ın tek bir sensörü olduğu ve Sensör'ün EkipmanId'yi anahtar olarak kullandığı yapı:
                entity.HasOne(e => e.Sensor)
                      .WithOne(s => s.Ekipman)
                      .HasForeignKey<Sensor>(s => s.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });



            // Ekskavatör Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<Ekskavator>(entity =>
            {
                // 1. ekipmanId'yi bu tablonun Primary Key'i yapıyoruz
                entity.HasKey(e => e.ekipmanId);

                // 2. Ondalık (decimal) alanlar için veritabanı hassasiyet ayarı
                // Bu ayar yapılmazsa EF Core hata verebilir veya veriyi yuvarlayabilir.
                entity.Property(e => e.kovaKapasitesi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.motorGucu).HasColumnType("decimal(18,2)");
                entity.Property(e => e.maksimumKaziDerinligi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.bomUzunlugu).HasColumnType("decimal(18,2)");

                entity.Property(e => e.plaka).HasMaxLength(20);
                entity.Property(e => e.paletTipi).HasMaxLength(50);

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // Ekskavator tablosundaki her satır, Ekipman tablosundaki bir satıra karşılık gelir.
                entity.HasOne(e => e.Ekipman)
                      .WithOne() // Eğer Ekipman.cs içinde 'public Ekskavator Ekskavator' yoksa boş bırakılır
                      .HasForeignKey<Ekskavator>(e => e.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse ekskavatör detayları da silinsin
            });



            // El Aletleri Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<ElAletleri>(entity =>
            {
                // 1. ekipmanId'yi bu tablonun Birincil Anahtarı (Primary Key) yapıyoruz
                entity.HasKey(e => e.ekipmanId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.gucKaynagiTipi)
                      .HasMaxLength(50);

                entity.Property(e => e.bataryaKapasitesi)
                      .HasColumnType("decimal(18,2)"); // Sayısal hassasiyet ayarı

                entity.Property(e => e.kullanimAmaci)
                      .HasMaxLength(200);

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // ElAletleri tablosundaki her satır, Ekipman tablosundaki bir satırla aynı ID'ye sahip olmalıdır.
                entity.HasOne(ea => ea.Ekipman)
                      .WithOne() // Ekipman sınıfında 'public ElAletleri ElAletleri' yoksa boş bırakılır
                      .HasForeignKey<ElAletleri>(ea => ea.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse el aleti detayı da silinsin
            });




            // Hafriyat Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<Hafriyat>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama
                entity.HasKey(e => e.ekipmanId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.plaka)
                      .HasMaxLength(20);

                // Sayısal (decimal) alanlar için veritabanı hassasiyeti (Hata almamak için kritik)
                entity.Property(e => e.damperHacmi)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.azamiYukAgirligi)
                      .HasColumnType("decimal(18,2)");

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // Hafriyat tablosundaki ID, Ekipman tablosundaki ID ile birebir eşleşmelidir.
                entity.HasOne(h => h.Ekipman)
                      .WithOne() // Ekipman tarafında 'public Hafriyat Hafriyat' yoksa boş bırakılır
                      .HasForeignKey<Hafriyat>(h => h.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse hafriyat detayları da silinsin
            });




            // Kepçe Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<Kepce>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama
                entity.HasKey(e => e.ekipmanId);

                // 2. Özellik Kısıtlamaları ve Hassasiyet Ayarları
                entity.Property(e => e.plaka)
                      .HasMaxLength(20);

                // Decimal alanlar için SQL hassasiyet ayarı (decimal(18,2))
                entity.Property(e => e.yuklemeKapasitesi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.kovaKapasitesi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.bosaltmaYuksekligi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.devrilmeYuku).HasColumnType("decimal(18,2)");

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // Kepce tablosundaki ID, Ekipman tablosundaki ID ile aynı olmalıdır.
                entity.HasOne(k => k.Ekipman)
                      .WithOne() // Ekipman tarafında 'public Kepce Kepce' özelliği yoksa boş bırakılır
                      .HasForeignKey<Kepce>(k => k.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse kepçe detayları da silinsin
            });




            // Kırıcı Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<Kirici>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama
                entity.HasKey(e => e.ekipmanId);

                // 2. Ondalık (decimal) alanlar için veritabanı hassasiyet ayarı
                // Maden ekipmanlarında hassas teknik değerler için bu ayar zorunludur.
                entity.Property(e => e.darbeEnerjisi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.dakikadakiDarbeSayisi).HasColumnType("decimal(18,2)");
                entity.Property(e => e.calismaBasinci).HasColumnType("decimal(18,2)");

                entity.Property(e => e.ucTipi).HasMaxLength(50);
                entity.Property(e => e.gerekenYagDebisi).HasMaxLength(50);

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // Kirici tablosundaki ID, Ekipman tablosundaki ID ile aynı olmalıdır.
                entity.HasOne(k => k.Ekipman)
                      .WithOne() // Ekipman sınıfında 'public Kirici Kirici' yoksa boş bırakılır
                      .HasForeignKey<Kirici>(k => k.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse kırıcı detayları da silinsin
            });



            // Sensör Yapılandırması (1:1 - Zayıf Varlık Yapısı)
            modelBuilder.Entity<Sensor>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama
                // Sensör'ün kendi Id'si yok, EkipmanId'yi anahtar olarak kullanıyor.
                entity.HasKey(e => e.ekipmanId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.sensorTipi)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.sensorDurumu)
                      .HasMaxLength(20);

                entity.Property(e => e.baglantiProtokolu)
                      .HasMaxLength(50);

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // Sensör tablosundaki her satır, Ekipman tablosundaki tam bir satıra karşılık gelir.
                entity.HasOne(s => s.Ekipman)
                      .WithOne(e => e.Sensor) // Ekipman sınıfındaki 'public Sensor Sensor { get; set; }' ile eşleşir
                      .HasForeignKey<Sensor>(s => s.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Ekipman silinirse sensör bilgisi de silinir
            });




            // Sensör Verisi Yapılandırması (Weak Entity Yapısı)
            modelBuilder.Entity<SensorVerisi>(entity =>
            {
                // 1. Birincil Anahtar
                entity.HasKey(e => e.sensorVerisiId);


                entity.Property(e => e.deger)
                      .HasColumnType("decimal(18,4)"); // 18 basamak toplam, 4 basamak virgülden sonrası için (Hassas ölçüm)

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.birim)
                      .HasMaxLength(20);

                entity.Property(e => e.olcumTarihi)
                      .IsRequired();

                // 3. Sensör/Ekipman ile İlişki (N:1)
                // Sensor tablosunun anahtarı ekipmanId olduğu için bağlantıyı buradan kuruyoruz.
                entity.HasOne(sv => sv.Sensor)
                      .WithMany() // Bir sensörün binlerce veri kaydı olabilir
                      .HasForeignKey(sv => sv.ekipmanId)
                      .OnDelete(DeleteBehavior.Cascade); // Cihaz silinirse ölçüm geçmişi de silinir

                // 4. Vardiya ile İlişki (N:1)
                // Verinin hangi vardiya esnasında üretildiğini takip etmek için.
                entity.HasOne(sv => sv.Vardiya)
                      .WithMany()
                      .HasForeignKey(sv => sv.vardiyaId)
                      .OnDelete(DeleteBehavior.Restrict); // Vardiya silinirken veriler korunmalı
            });




            // Log Kaydı Yapılandırması
            modelBuilder.Entity<LogKaydi>(entity =>
            {
                // 1. Birincil Anahtar
                entity.HasKey(e => e.logKaydiID);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.islemTipi)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.ipAdresi)
                      .HasMaxLength(45); // IPv6 destekleyecek uzunlukta

                entity.Property(e => e.onemSeviyesi)
                      .HasMaxLength(20);

                entity.Property(e => e.logKaydiAciklamasi)
                      .HasColumnType("text"); // Uzun detaylar için

                // 3. Personel ile İlişki (N:1)
                entity.HasOne(l => l.Personel)
                      .WithMany() // Personel silinse de loglar denetim (audit) için kalmalı
                      .HasForeignKey(l => l.personelId)
                      .OnDelete(DeleteBehavior.SetNull); // Personel silinirse bu alan null olsun, log silinmesin

                // 4. Ekipman ile İlişki (N:1)
                entity.HasOne(l => l.Ekipman)
                      .WithMany()
                      .HasForeignKey(l => l.ekipmanId)
                      .OnDelete(DeleteBehavior.SetNull); // Ekipman silinse de log geçmişi saklanmalı
            });





            // Takip Cihazı Yapılandırması
            modelBuilder.Entity<TakipCihazi>(entity =>
            {
                // 1. Birincil Anahtar
                entity.HasKey(e => e.takipCihaziId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.takipCihaziSeriNo)
                      .HasMaxLength(50)
                      .IsRequired();

                entity.Property(e => e.takipCihaziDurumu)
                      .HasMaxLength(20);

                entity.Property(e => e.pilSeviyesi)
                      .HasColumnType("decimal(5,2)"); // Örn: %100.00 şeklinde saklamak için

                // 3. Personel ile İlişki (N:1)
                entity.HasOne(t => t.Personel)
                      .WithMany() // Bir personelin birden fazla takip cihazı taşıması senaryosuna açık
                      .HasForeignKey(t => t.personelId)
                      .OnDelete(DeleteBehavior.SetNull); // Personel silinirse cihaz 'boşta' konumuna düşer

                // 4. Ekipman ile İlişki (N:1)
                entity.HasOne(t => t.Ekipman)
                      .WithMany()
                      .HasForeignKey(t => t.ekipmanId)
                      .OnDelete(DeleteBehavior.SetNull); // Ekipman silinirse cihaz kaydı korunur, FK null olur
            });



            // --- 3. VARDİYA & TAKİP MODÜLÜ ---
            // Vardiya Yapılandırması
            modelBuilder.Entity<Vardiya>(entity =>
            {
                entity.HasKey(e => e.vardiyaId);

                entity.Property(e => e.vardiyaAdi).HasMaxLength(100).IsRequired();
                entity.Property(e => e.vardiyaNotlari).HasColumnType("text");

                // 1. Vardiya Sorumlusu (N:1)
                entity.HasOne(v => v.VardiyaSorumlusuPersonel)
                      .WithMany() // Personel tarafında bir koleksiyona gerek yoksa boş bırakılır
                      .HasForeignKey(v => v.vardiyaSorumlusu)
                      .OnDelete(DeleteBehavior.Restrict);

                // 2. ISG Sorumlusu (N:1)
                entity.HasOne(v => v.IsgSorumlusuPersonel)
                      .WithMany()
                      .HasForeignKey(v => v.vardiyaIsgSorumlusu)
                      .OnDelete(DeleteBehavior.Restrict);

                // 3. Teknik Sorumlu (N:1)
                entity.HasOne(v => v.TeknikSorumlusuPersonel)
                      .WithMany()
                      .HasForeignKey(v => v.vardiyaTeknikSorumlusu)
                      .OnDelete(DeleteBehavior.Restrict);

                // 4. Ekipman İlişkisi (N:1)
                entity.HasOne(v => v.Ekipman)
                      .WithMany()
                      .HasForeignKey(v => v.ekipmanId)
                      .OnDelete(DeleteBehavior.SetNull); // Ekipman silinse de vardiya kaydı kalmalı
            });




            // Ekip Yapılandırması
            modelBuilder.Entity<Ekip>(entity =>
            {
                    // 1. Birincil Anahtar
                    entity.HasKey(e => e.ekipId);

                    // 2. Özellik Kısıtlamaları
                    entity.Property(e => e.personelGorevi)
                          .HasMaxLength(50);

                    entity.Property(e => e.durum)
                          .HasMaxLength(20);

                    // 3. Personel ile İlişki (Foreign Key: personelId)
                    // Bir personel bir ekip kaydında yer alır.
                    entity.HasOne(e => e.Personel)
                          .WithMany() // Eğer Personel sınıfında 'ICollection<Ekip> Ekipler' varsa buraya eklenir
                          .HasForeignKey(e => e.personelId)
                          .OnDelete(DeleteBehavior.Restrict); // Personel silinince ekip geçmişi bozulmasın

                    // 4. Vardiya ile İlişki (Foreign Key: vardiyaId)
                    // Bu ekip kaydı belirli bir vardiyaya aittir.
                    entity.HasOne(e => e.Vardiya)
                          .WithMany() // Eğer Vardiya sınıfında 'ICollection<Ekip> Ekipler' varsa buraya eklenir
                          .HasForeignKey(e => e.vardiyaId)
                          .OnDelete(DeleteBehavior.Cascade); // Vardiya silinirse o vardiyaya bağlı ekip tanımları silinsin
            });



            // Vaka Yapılandırması
            modelBuilder.Entity<Vaka>(entity =>
            {
                // 1. Birincil Anahtar
                entity.HasKey(e => e.vakaId);

                // 2. Metin Alanı Kısıtlamaları
                entity.Property(e => e.vakaAdi).HasMaxLength(100).IsRequired();
                entity.Property(e => e.vakaAciklamasi).HasColumnType("text");

                // 3. İlişki Tanımı: Raporlayan Personel (N:1)
                entity.HasOne(v => v.Personel)
                      .WithMany()
                      .HasForeignKey(v => v.personelId)
                      .OnDelete(DeleteBehavior.Restrict); // Personel silinse de vaka kaydı kalmalı

                // 4. İlişki Tanımı: Raporlayan Sensör/Ekipman (N:1)
                entity.HasOne(v => v.RaporlayanEkipman)
                      .WithMany()
                      .HasForeignKey(v => v.raporlayanEkipmanId)
                      .OnDelete(DeleteBehavior.Restrict);

                // 5. İlişki Tanımı: Olayın Gerçekleştiği Ekipman (N:1)
                entity.HasOne(v => v.IlgiliEkipman)
                      .WithMany()
                      .HasForeignKey(v => v.ilgiliEkipmanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });




            // Aksiyon (Müdahale) Yapılandırması
            modelBuilder.Entity<Aksiyon>(entity =>
            {
                        // 1. Birincil Anahtar (Primary Key)
                        entity.HasKey(e => e.mudahaleId);

                        // 2. Özellik Kısıtlamaları
                        entity.Property(e => e.mudahaleTuru)
                              .HasMaxLength(50);

                        entity.Property(e => e.uygulananCozum)
                              .HasColumnType("text"); // Uzun metinler için SQL 'text' tipi

                        // 3. Vaka ile İlişki (Foreign Key: vakaId)
                        // Her müdahale mutlaka bir vakaya bağlıdır (Bire-Çok)
                        entity.HasOne(a => a.Vaka)
                              .WithMany() // Bir vaka için birden fazla aksiyon/müdahale kaydı olabilir
                              .HasForeignKey(a => a.vakaId)
                              .OnDelete(DeleteBehavior.Cascade); // Vaka silinirse müdahale kayıtları da silinir

                        // 4. Ekip ile İlişki (Foreign Key: ekipId)
                        // Müdahaleyi yapan ekibin bağlantısı
                        entity.HasOne(a => a.Ekip)
                              .WithMany() // Bir ekip tarih boyunca birçok müdahalede bulunmuş olabilir
                              .HasForeignKey(a => a.ekipId)
                              .OnDelete(DeleteBehavior.Restrict); // Ekip silinse bile geçmiş müdahale kayıtları silinmesin

                        // 5. Zaman Verileri Zorunluluğu
                        entity.Property(e => e.mudahaleBaslangicSaati)
                              .IsRequired();
            });




            // Rapor Yapılandırması
            modelBuilder.Entity<Rapor>(entity =>
            {
                entity.HasKey(e => e.raporId);

                entity.Property(e => e.raporAdi).HasMaxLength(100).IsRequired();
                entity.Property(e => e.raporDosyaYolu).HasMaxLength(500);

                // 1. Personel İlişkisi (N:1)
                entity.HasOne(r => r.Personel)
                      .WithMany() // Personel silinse de rapor arşivi kalmalı
                      .HasForeignKey(r => r.personelId)
                      .OnDelete(DeleteBehavior.SetNull); // Personel silinirse ID null olur, rapor silinmez

                // 2. Ekipman/Sensör İlişkisi (N:1)
                // Otomatik sistem raporları için bağlantı
                entity.HasOne(r => r.Ekipman)
                      .WithMany()
                      .HasForeignKey(r => r.ekipmanId)
                      .OnDelete(DeleteBehavior.SetNull); // Ekipman silinse de geçmiş raporlar saklanır
            });


            // Ekipman Raporu Yapılandırması
            modelBuilder.Entity<EkipmanRaporu>(entity =>
            {
                // 1. raporId'yi bu tablonun Birincil Anahtarı (PK) yapıyoruz
                entity.HasKey(e => e.raporId);

                // 2. raporId aynı zamanda Rapor tablosuna bağlı bir Yabancı Anahtardır (FK)
                // Bu, 1:1 (Bire-Bir) bir ilişkidir
                entity.HasOne(er => er.Rapor)
                      .WithOne() // Rapor sınıfında EkipmanRaporu navigation property'si yoksa boş kalabilir
                      .HasForeignKey<EkipmanRaporu>(er => er.raporId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ekipmanTuru).HasMaxLength(50);
            });



            // Personel Raporu Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<PersonelRaporu>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama
                // Kendi ID'si yok, RaporId üzerinden kimlik kazanıyor.
                entity.HasKey(e => e.raporId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.uzmanlikAlani)
                      .HasMaxLength(100);

                // Sayısal hassasiyet (decimal 18,2) ayarı
                entity.Property(e => e.calismaSuresi)
                      .HasColumnType("decimal(18,2)");

                // 3. İlişki Tanımı: Bire-Bir (1:1)
                // PersonelRaporu tablosundaki her satır, Rapor tablosundaki tam bir satıra karşılık gelir.
                entity.HasOne(pr => pr.Rapor)
                      .WithOne() // Eğer Rapor sınıfında 'public PersonelRaporu PersonelRaporu' yoksa boş bırakılır
                      .HasForeignKey<PersonelRaporu>(pr => pr.raporId)
                      .OnDelete(DeleteBehavior.Cascade); // Ana rapor silinirse bu detay rapor da silinsin
            });



            // Vaka Raporu Yapılandırması (Table-per-Type Miras Yapısı)
            modelBuilder.Entity<VakaRaporu>(entity =>
            {
                // 1. Birincil Anahtar Tanımlama (Shared Primary Key)
                entity.HasKey(e => e.raporId);

                // 2. Özellik Kısıtlamaları
                entity.Property(e => e.ciddiyetSeviyesi).HasMaxLength(20);
                entity.Property(e => e.cozumSuresi).HasColumnType("decimal(18,2)");

                // 3. Raporlayan Personel ile İlişki (N:1)
                entity.HasOne(vr => vr.Personel)
                      .WithMany()
                      .HasForeignKey(vr => vr.personelId)
                      .OnDelete(DeleteBehavior.Restrict); // Personel silinse de resmi raporlar kalmalı

                // 4. Raporlayan Ekipman/Sensör ile İlişki (N:1)
                entity.HasOne(vr => vr.RaporlayanEkipman)
                      .WithMany()
                      .HasForeignKey(vr => vr.raporlayanEkipmanId)
                      .OnDelete(DeleteBehavior.Restrict);

                // 5. Ana Rapor ile Bire-Bir İlişki (1:1)
                entity.HasOne(vr => vr.Rapor)
                      .WithOne() // Rapor tarafında VakaRaporu navigation property'si yoksa boş kalabilir
                      .HasForeignKey<VakaRaporu>(vr => vr.raporId)
                      .OnDelete(DeleteBehavior.Cascade); // Ana rapor silinirse detay rapor da silinsin
            });







            //SEED DATA - ÖRNEK VERİLER

            // 1. Personel Seed
            modelBuilder.Entity<Personel>().HasData(
                new Personel
                {
                    personelId = 1,
                    personelAdi = "Büşra",
                    personelSoyadi = "Arslan",
                    tckn = "12345678901",
                    uzmanlik = "Mühendis",
                    personelDurumu = "Aktif",
                    iseGirisTarihi = new DateTime(2025, 01, 01),
                    eposta = "busra@minemation.com",
                    sifreHash = "AQAAAAEAACcQAAAAE...",
                    calisanTipi = "Tam Zamanlı",
                    rfidKartNumarasi = "RFID-001",
                    kullaniciRolu = "Admin",
                    departman = "Yönetim",
                    calismaKonumu = "Merkez Sahası",
                    cinsiyet = "Kadın",
                    telNo = "05551112233",
                    ikinciTelNo = "05551112234",
                    adres = "Ankara",
                    sonGirisTarihi = new DateTime(2026, 05, 09, 10, 00, 00) // Sabitlendi
                }
            );

            // 2. Sağlık Bilgileri Seed
            modelBuilder.Entity<SaglikBilgileri>().HasData(
                new SaglikBilgileri
                {
                    personelId = 1,
                    kanGrubu = "A Rh+",
                    saglikDurumu = "Genel sağlık durumu iyi.",
                    kronikHastaliklar = new List<string>(),
                    alerjiler = new List<string> { "Polen" },
                    saglikCalismaKisitlamalari = "Yok",
                    acilDurumNotu = "İlaç alerjisi yok.",
                    sonMuayeneTarihi = new DateTime(2026, 02, 15) // Sabitlendi
                }
            );

            // 3. Ekipman Seed
            modelBuilder.Entity<Ekipman>().HasData(
                new Ekipman
                {
                    ekipmanId = 101,
                    ekipmanAdi = "Dev Ekskavatör CAT-395",
                    ekipmanMarka = "Caterpillar",
                    ekipmanModel = "395-L",
                    durum = "Aktif",
                    seriNo = "SN-CAT-999",
                    RFIDEtiket = "TAG-EQ-101",
                    ureticiFirma = "CAT Global",
                    tedarikciFirma = "Borusan Makina",
                    uretimYili = new DateTime(2023, 1, 1),
                    boyut = 12.50m,
                    agirlik = 94000.00m,
                    satinAlmaTarihi = new DateTime(2024, 1, 1),
                    garantiBaslangicTarihi = new DateTime(2024, 1, 1),
                    teknikDokuman = "/docs/cat395.pdf",
                    kullanimKilavuzu = "/docs/manual.pdf",
                    garantiBelgesi = "/docs/warranty.pdf",
                    bakimFormu = "/docs/service.pdf",
                    satinAlmaBelgesi = "/docs/invoice.pdf",
                    operasyonTuru = "Kazı ve Yükleme",
                    sonBakimTarihi = new DateTime(2026, 04, 01), // Sabitlendi
                    gelecekBakimTarihi = new DateTime(2026, 10, 01) // Sabitlendi
                }
            );

            // 4. Ekskavatör Detay Seed
            modelBuilder.Entity<Ekskavator>().HasData(
                new Ekskavator
                {
                    ekipmanId = 101,
                    plaka = "06 MINE 01",
                    kovaKapasitesi = 6.5m,
                    motorGucu = 543m,
                    maksimumKaziDerinligi = 9.7m,
                    paletTipi = "Çelik Ağır Hizmet",
                    bomUzunlugu = 7.2m
                }
            );

            // 5. Sensör Seed
            modelBuilder.Entity<Sensor>().HasData(
                new Sensor
                {
                    ekipmanId = 101,
                    sensorTipi = "Sıcaklık ve Basınç",
                    sensorDurumu = "Aktif",
                    minEsikDeger = 20.0,
                    maxEsikDeger = 110.0,
                    hassasiyet = 0.5,
                    baglantiProtokolu = "MQTT",
                    haberlesmeTipi = "Kablosuz 4G"
                }
            );

            // 6. Vardiya Seed
            modelBuilder.Entity<Vardiya>().HasData(
                new Vardiya
                {
                    vardiyaId = 1,
                    vardiyaAdi = "Gündüz Vardiyası A",
                    vardiyaTanimi = "08:00 - 16:00 Kazı Operasyonu",
                    vardiyaBaslangicTarihi = new DateTime(2026, 05, 09, 08, 00, 00), // Sabitlendi
                    vardiyaBitisTarihi = new DateTime(2026, 05, 09, 16, 00, 00),    // Sabitlendi
                    vardiyaOlusturmaTarihi = new DateTime(2026, 05, 09, 07, 30, 00), // Sabitlendi
                    vardiyaSupervizoru = "Büşra Arslan",
                    personelSayisi = 5,
                    ekipmanSayisi = 2,
                    ekipSayisi = 1,
                    vardiyaDurumu = "Aktif",
                    vardiyaTipi = "Üretim",
                    toplaVardiyaSuresi = 8,
                    calismaBolgesi = "Batı Ocağı",
                    operasyonTipi = "Dekapaj",
                    operasyonRiskSeviyesi = "Orta",
                    vardiyaNotlari = "Zemin kaygan, dikkatli olunmalı.",
                    ekipmanOperatoru = "Ahmet Operatör",
                    vardiyaSorumlusu = 1,
                    vardiyaIsgSorumlusu = 1,
                    vardiyaTeknikSorumlusu = 1
                }
            );

            // 7. Ekip Seed
            modelBuilder.Entity<Ekip>().HasData(
                new Ekip
                {
                    ekipId = 1,
                    ekipUyeSayisi = 1,
                    personelGorevi = "Vardiya Amiri",
                    durum = "Görevde",
                    personelId = 1,
                    vardiyaId = 1
                }
            );

            // 8. Vaka Seed
            modelBuilder.Entity<Vaka>().HasData(
                new Vaka
                {
                    vakaId = 1,
                    vakaAdi = "Yüksek Motor Sıcaklığı",
                    vakaTuru = "Teknik Arıza",
                    vakaCiddiyetSeviyesi = "Kritik",
                    vakaDurumu = "İncelemede",
                    vakaAciklamasi = "Sensör verisi normalin üzerine çıktı.",
                    vakaOlusmaTarihi = new DateTime(2026, 05, 09, 09, 15, 00), // Sabitlendi
                    olayNedeni = "Soğutma sıvısı sızıntısı şüphesi",
                    personelId = 1,
                    ilgiliEkipmanId = 101
                }
            );

        }
    }
}