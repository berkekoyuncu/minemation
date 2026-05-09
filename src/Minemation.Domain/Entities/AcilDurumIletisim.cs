using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class AcilDurumIletisim
    {
        public int acilDurumKisisiId {  get; set; }
        public string acilDurumKisileriAd {  get; set; }
        public string acilDurumKisileriSoyad { get; set; }
        public string acilDurumKisileriYakinlik { get; set; }
        public string acilDurumKisileriTelNo { get; set; }

        //foreign key belirtimi 
        public int personelId { get; set; }
        public Personel Personel { get; set; }
    }
}
