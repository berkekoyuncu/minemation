using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Sensor : Ekipman
    {
        public string sensorTipi {  get; set; }
        public string sensorDurumu { get; set; }
        public double minEsikDeger { get; set; }
        public double maxEsikDeger { get; set; }
        public double hassasiyet {  get; set; }
        public string baglantiProtokolu {  get; set; }
        public string haberlesmeTipi { get; set; }

        // --- İlişkiler ---
        // Bir sensörün geçmişe dönük birçok ölçüm verisi olabilir
        public virtual ICollection<SensorVerisi> SensorVerileri { get; set; } = new List<SensorVerisi>();

    }
}
