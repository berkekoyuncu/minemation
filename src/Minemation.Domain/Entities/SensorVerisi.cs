using System;
using System.Collections.Generic;
using System.Text;
using System;


namespace Minemation.Domain.Entities
{
    public class SensorVerisi
    {
        public int sensorVerisiId { get; set; }
        public decimal deger { get; set; }
        public string birim { get; set; }
        public DateTime olcumTarihi { get; set; }

        // Sadece SensorId (ekipmanId) kalmalı
        public int ekipmanId { get; set; }
        public virtual Sensor Sensor { get; set; }

        public int vardiyaId { get; set; }
        public virtual Vardiya Vardiya { get; set; }
    }
}
