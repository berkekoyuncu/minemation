using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Ekipman
    {
        public int ekipmanId { get; set; }
        public string ekipmanAdi { get; set; }
        public string ekipmanMarka { get; set; }
        public string ekipmanModel { get; set; }
        public string durum { get; set; }
        public DateTime sonBakimTarihi { get; set; }
        public DateTime gelecekBakimTarihi { get; set; }
        public string seriNo { get; set; }
        public string RFIDEtiket { get; set; }
        public string ureticiFirma { get; set; }
        public string tedarikciFirma { get; set; }
        public DateTime uretimYili { get; set; }

        public decimal boyut { get; set; }
        public decimal agirlik { get; set; }

        public DateTime satinAlmaTarihi { get; set; }
        public DateTime garantiBaslangicTarihi { get; set; }
        public string teknikDokuman { get; set; }
        public string kullanimKilavuzu { get; set; }
        public string garantiBelgesi { get; set; }
        public string bakimFormu { get; set; }
        public string satinAlmaBelgesi { get; set; }
        public string operasyonTuru { get; set; }



        public Sensor Sensor { get; set; }

    }
}
