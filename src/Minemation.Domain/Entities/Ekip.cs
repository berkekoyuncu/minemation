using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities
{
    public class Ekip
    {
        public int ekipId {  get; set; }
        public int ekipUyeSayisi {  get; set; }
        public string personelGorevi { get; set; }
        public string durum {  get; set; }

        //foreign key belirtimi
        public int personelId { get; set; }
        public Personel Personel { get; set; }

        public int vardiyaId { get; set; }
        public Vardiya Vardiya { get; set; }

    }
}
