using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class VakaRaporu
    {
        // Shared Primary Key: Hem bu tablonun PK'sı hem de Rapor tablosuna giden FK.
        public int raporId { get; set; }

        public string ciddiyetSeviyesi { get; set; }

        public decimal cozumSuresi { get; set; }

        // --- Raporlayan Tarafın Belirtilmesi ---

        // Raporu hazırlayan personel (Opsiyonel)
        public int? personelId { get; set; }
        public virtual Personel Personel { get; set; }

        // Rapor otomatik bir sensör vaka kaydından besleniyorsa (Opsiyonel)
        public int? raporlayanEkipmanId { get; set; }
        public virtual Ekipman RaporlayanEkipman { get; set; }

        // Navigasyon Özelliği: Üst Rapor sınıfına erişim
        public virtual Rapor Rapor { get; set; }
    }
}