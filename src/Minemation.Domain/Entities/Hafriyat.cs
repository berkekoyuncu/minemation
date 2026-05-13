using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Hafriyat : Ekipman
    {
        public string plaka {  get; set; }
        public decimal damperHacmi { get; set; }
        public decimal azamiYukAgirligi { get; set; }
        public int dingilSayisi {  get; set; }

    }
}
