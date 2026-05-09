using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class PersonelRaporu
    {
        public int raporId { get; set; }

        public string uzmanlikAlani { get; set; }

        public int personelSayisi { get; set; }

        public decimal calismaSuresi { get; set; }



        public virtual Rapor Rapor { get; set; }
    }
}
