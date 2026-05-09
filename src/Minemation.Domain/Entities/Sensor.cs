using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities
{
    public class Sensor
    {
        public int ekipmanId { get; set; }

        public string sensorTipi {  get; set; }
        public string sensorDurumu { get; set; }
        public double minEsikDeger { get; set; }
        public double maxEsikDeger { get; set; }
        public double hassasiyet {  get; set; }
        public string baglantiProtokolu {  get; set; }
        public string haberlesmeTipi { get; set; }



    // Foreign key
        public virtual Ekipman Ekipman { get; set; }
    }
}
