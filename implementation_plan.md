# Minemation GUI Implementation Plan

Bu plan, `docs/screenshots` klasöründeki taslaklara (BM314-02, BM314-03, BM314-04) dayalı olarak WPF (Masaüstü) ve .NET MAUI (Mobil) arayüzlerinin modern, "premium" ve estetik bir şekilde geliştirilmesini kapsar.

## User Review Required

> [!IMPORTANT]
> Arayüz tasarımlarında **Premium Endüstriyel Karanlık Tema (Dark Mode)** kullanılmasını öneriyorum. Maden otomasyonu konseptine uygun olarak derin lacivert/gri arka planlar (`#0F172A`) ve uyarı/dikkat çeken yerler için Elektrik Mavisi (`#06B6D4`) veya Güvenlik Turuncusu (`#F97316`) gibi parlak vurgu renkleri kullanmayı hedefliyorum. Cam efekti (glassmorphism) ve yumuşak gölgelerle arayüzü modernleştireceğiz.
> Bu tasarım dili sizin için uygun mu, yoksa daha klasik bir aydınlık (Light) tema mı tercih edersiniz?

## Open Questions

1.  **İkonlar:** Arayüzde kullanılacak ikonlar için `MaterialDesignThemes` (WPF) ve FontAwesome veya Material Icons (MAUI) kullanmayı planlıyorum. Uygun mudur?
2.  **Arka Plan Görseli:** Login ekranının (BM314-02) sol tarafındaki "Arka Fon" için projedeki `photo1.jpeg` görselini mi kullanalım, yoksa yapay zeka ile modern bir maden sahası görseli (generate_image aracı ile) mi üretelim?
3.  **Başlangıç:** Önce Masaüstü (WPF) tarafındaki Login ve Personel ekranlarıyla mı başlayalım, yoksa MAUI (Mobil) tarafıyla mı?

## Proposed Changes

### Design System & Theming

Ortak bir tasarım dili oluşturulacak.

#### [MODIFY] `Minemation.Desktop/Resources/Colors.xaml`
- Ortak renk paletinin tanımlanması (Primary, Background, Surface, Text renkleri).
- Modern Control (Button, TextBox, DataGrid) stillerinin `App.xaml` içine entegre edilmesi.

#### [MODIFY] `Minemation.Mobile/Resources/Styles/Colors.xaml` & `Styles.xaml`
- Masaüstü ile uyumlu renk paleti ve kontrol stillerinin MAUI tarafına aktarılması.

---

### Minemation.Desktop (WPF)

Masaüstü uygulaması için BM314 taslakları uygulanacaktır.

#### [MODIFY] `src/Minemation.Desktop/LoginView.xaml`
- **Referans:** BM314-02.jpg
- Sol tarafta tam ekran görsel ve logo/motto.
- Sağ tarafta blur (glassmorphism) efektli şık bir giriş formu (T.C. Kimlik No ve Şifre).
- Yuvarlatılmış köşeler ve hover animasyonlu giriş butonu.

#### [NEW] `src/Minemation.Desktop/MainWindow.xaml` (veya MainDashboard)
- **Referans:** BM314-03.jpg
- Üst kısımda modern bir navigasyon bar (Logo, Home ikonu, Personel, Vardiya, Ekipman vs. sekmeleri).
- Sağ üstte kullanıcı profil bileşeni.

#### [NEW] `src/Minemation.Desktop/PersonnelView.xaml`
- **Referans:** BM314-04.jpg
- Alt sekmeler ("İdari Personel", "Saha Personeli").
- Özel stillendirilmiş, karanlık temaya uygun modern bir `DataGrid` (No, Ad, Soyad, Uzmanlık, Vardiya, Durum, Konum).
- Alt kısımda filtreleme araçları ve "+ Ekle" butonu.

---

### Minemation.Mobile (.NET MAUI)

Mobil uygulama masaüstünün yeteneklerini dar ekranlar için optimize edecektir.

#### [NEW] `src/Minemation.Mobile/Views/LoginPage.xaml`
- Masaüstü login ekranının mobil uyumlu (dikey) versiyonu.

#### [MODIFY] `src/Minemation.Mobile/AppShell.xaml`
- Masaüstündeki üst menünün yerine, mobil dostu bir alt gezinme çubuğu (Bottom TabBar) veya yan menü (Flyout) eklenecek (Personel, Vardiya, Ekipman).

#### [NEW] `src/Minemation.Mobile/Views/PersonnelPage.xaml`
- DataGrid yerine kart tabanlı, modern bir `CollectionView`.
- Ekleme işlemi için sağ alta bir Yüzen Aksiyon Butonu (Floating Action Button - FAB).

## Verification Plan

### Automated Tests
- Ekranların render hataları olmadan derlenmesi.
- Buton tıklama animasyonları ve form validasyonlarının test edilmesi.

### Manual Verification
- `dotnet run` veya Visual Studio ile hem WPF hem de MAUI projeleri başlatılıp tasarımların responsive olup olmadığı ve karanlık tema uyumluluğu kontrol edilecektir.
