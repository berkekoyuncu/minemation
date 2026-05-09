using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class EkipmanRaporu
    {
        public string ekipmanTuru {  get; set; }
        public int arizaSayisi { get; set; }

        public int calismaSuresi { get; set; }


        //foreign key belirtimi
        public int raporId { get; set; }

        public Rapor Rapor { get; set; }

    }
}
