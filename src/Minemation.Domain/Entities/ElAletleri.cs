using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class ElAletleri
    {
        public int ekipmanId { get; set; }

        public string gucKaynagiTipi {  get; set; }
        public decimal bataryaKapasitesi { get; set; }
        public string kullanimAmaci {  get; set; }



        public virtual Ekipman Ekipman { get; set; }
    }
}
