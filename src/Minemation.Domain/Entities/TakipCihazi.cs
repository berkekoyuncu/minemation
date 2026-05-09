using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class TakipCihazi
    {
        public int takipCihaziId {  get; set; }
        public string takipCihaziSeriNo { get; set; }
        public string takipCihaziTuru {  get; set; }
        public string takipCihaziModeli { get; set; }
        public string takipCihaziDurumu {  get; set; }
        public DateTime takipCihaziSonBaglantiZamani { get; set; }
        public string takipCihaziHaberlesmeProtokolu { get; set; }
        public decimal pilSeviyesi {  get; set; }

        // --- Foreign Key Bağlantıları ---
        // personelin olabilir
        public int? personelId { get; set; }
        public virtual Personel Personel { get; set; }

        // ekipmanın olabilir 
        public int? ekipmanId { get; set; }
        public virtual Ekipman Ekipman { get; set; }
    }
}
