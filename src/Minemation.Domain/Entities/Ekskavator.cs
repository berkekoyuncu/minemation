using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class Ekskavator
    {
        public int ekipmanId { get; set; }

        public string plaka {  get; set; }
        public decimal kovaKapasitesi { get; set; }
        public decimal motorGucu {  get; set; }
        public decimal maksimumKaziDerinligi { get; set; }
        public string paletTipi {  get; set; }
        public decimal bomUzunlugu  { get; set; }

        //foreign key belirtimi

        public virtual Ekipman Ekipman { get; set; }

    }
}
