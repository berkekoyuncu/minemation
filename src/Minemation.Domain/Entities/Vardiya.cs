using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Vardiya
    {
        public int vardiyaId { get; set; }
        public string vardiyaAdi { get; set; }

        public string vardiyaTanimi { get; set; }
        public DateTime vardiyaBaslangicTarihi { get; set; }
        public DateTime vardiyaBitisTarihi { get; set; }
        public DateTime vardiyaOlusturmaTarihi { get; set; }
        public string vardiyaSupervizoru { get; set; }
        public int personelSayisi { get; set; }
        public int ekipmanSayisi { get; set; }
        public int ekipSayisi { get; set; }
        public string vardiyaDurumu { get; set; }
        public string vardiyaTipi { get; set; }
        public int toplaVardiyaSuresi { get; set; }
        public string calismaBolgesi { get; set; }
        public string operasyonTipi { get; set; }
        public string operasyonRiskSeviyesi { get; set; }
        public string vardiyaNotlari { get; set; }
        public string ekipmanOperatoru { get; set; }

        public int vardiyaSorumlusu { get; set; }
        public int vardiyaIsgSorumlusu { get; set; }
        public int vardiyaTeknikSorumlusu { get; set; }

        public virtual Personel VardiyaSorumlusuPersonel { get; set; }
        public virtual Personel IsgSorumlusuPersonel { get; set; }
        public virtual Personel TeknikSorumlusuPersonel { get; set; }
    }
}