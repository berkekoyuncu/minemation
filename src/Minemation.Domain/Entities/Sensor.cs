using System;
using System.Collections.Generic;
using System.Text;


namespace Minemation.Domain.Entities;

public class Sensor
{
    public string sensorTipi { get; set; } = string.Empty;
    public string sensorDurumu { get; set; } = string.Empty;

    public double minEsikDeger { get; set; }
    public double maxEsikDeger { get; set; }
    public double hassasiyet { get; set; }

    public string baglantiProtokolu { get; set; } = string.Empty;
    public string haberlesmeTipi { get; set; } = string.Empty;

    // Foreign key
    public int ekipmanId { get; set; }

    public virtual Ekipman Ekipman { get; set; } = null!;
}
