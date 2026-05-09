using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Kepce
    {
        public int ekipmanId { get; set; }


        public string plaka { get; set; }
        public decimal yuklemeKapasitesi { get; set; }
        public decimal kovaKapasitesi { get; set; }
        public decimal bosaltmaYuksekligi { get; set; }
        public decimal devrilmeYuku { get; set; }



        public Ekipman Ekipman { get; set; }
    }
    // uml ve er diyagrm arasında farklılıklar var
}
