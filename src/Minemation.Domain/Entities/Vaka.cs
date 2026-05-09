using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Minemation.Domain.Entities
{
    public class Vaka
    {
        public int vakaId { get; set; }

        public string vakaTuru { get; set; }
        public string vakaAdi { get; set; }
        public string vakaCiddiyetSeviyesi { get; set; }
        public string vakaDurumu { get; set; }
        public string vakaAciklamasi { get; set; }

        public DateTime vakaOlusmaTarihi { get; set; }
        public DateTime? vakaKapanmaTarihi { get; set; } // Vaka henüz kapanmamış olabilir
        public string olayNedeni { get; set; }

        // --- Foreign Key Bağlantıları ---

        // Vakayı raporlayan bir personel olabilir 
        public int? personelId { get; set; }
        public virtual Personel Personel { get; set; }

        // Vaka otomatik olarak bir sensör/ekipman tarafından tetiklenmiş olabilir 
        public int? raporlayanEkipmanId { get; set; }
        public virtual Ekipman RaporlayanEkipman { get; set; }

        // Vakanın üzerinde gerçekleştiği asıl ekipman (Örn: Ekskavatör motor arızası)
        public int? ilgiliEkipmanId { get; set; }
        public virtual Ekipman IlgiliEkipman { get; set; }
    }
}
