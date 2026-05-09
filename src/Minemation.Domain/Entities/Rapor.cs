using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Minemation.Domain.Entities
{
    public class Rapor
    {
        public int raporId { get; set; }

        public string raporAdi { get; set; }
        public string raporTuru { get; set; }

        public DateTime raporOlusturmaTarihi { get; set; }

        public string raporAciklamasi { get; set; }
        public string raporDosyaYolu { get; set; }
        public string zamanAraligi { get; set; }

        // --- Foreign Key Bağlantıları ---

        // Raporu bir personel oluşturmuş olabilir (Opsiyonel)
        public int? personelId { get; set; }
        public virtual Personel Personel { get; set; }

        // Rapor otomatik olarak bir ekipmandan/sensörden gelmiş olabilir (Opsiyonel)
        public int? ekipmanId { get; set; }
        public virtual Ekipman Ekipman { get; set; }
    }
}
