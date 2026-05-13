using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class ElAletleri : Ekipman
    {

        public string gucKaynagiTipi {  get; set; }
        public decimal bataryaKapasitesi { get; set; }
        public string kullanimAmaci {  get; set; }

    }
}
