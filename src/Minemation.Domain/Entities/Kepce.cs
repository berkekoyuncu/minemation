using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Kepce
    {
        public string plaka { get; set; }
        public decimal yuklemeKapasitesi { get; set; }
        public decimal kovaKapasitesi { get; set; }
        public decimal bosaltmaYuksekligi { get; set; }
        public decimal devrilmeYuku { get; set; }
        public int ekipmanId { get; set; }
        public Ekipman Ekipman { get; set; }

    }
}
