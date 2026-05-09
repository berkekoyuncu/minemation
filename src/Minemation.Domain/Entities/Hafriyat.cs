using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Hafriyat
    {
        public int ekipmanId { get; set; }


        public string plaka {  get; set; }
        public decimal damperHacmi { get; set; }
        public decimal azamiYukAgirligi { get; set; }
        public int dingilSayisi {  get; set; }

        public virtual Ekipman Ekipman { get; set; }
    }
}
