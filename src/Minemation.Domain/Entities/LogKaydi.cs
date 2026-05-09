using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class LogKaydi
    {
        // Birincil Anahtar
        public int logKaydiID { get; set; }

        public string islemTipi { get; set; }
        public string logKaydiAciklamasi { get; set; }

        public DateTime logKaydiTarihi { get; set; }

        public string ipAdresi { get; set; }
        public string onemSeviyesi { get; set; } // 'ö' karakteri 'o' yapıldı
        public string durum { get; set; }

        // Yabancı Anahtarlar (Foreign Keys)
        // int? (nullable) yaparak bir logun ya personele, ya ekipmana ya da sisteme ait olabilmesini sağladık.
        public int? personelId { get; set; }
        public virtual Personel Personel { get; set; }

        public int? ekipmanId { get; set; }
        public virtual Ekipman Ekipman { get; set; }
    }
}