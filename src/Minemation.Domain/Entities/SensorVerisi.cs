using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class SensorVerisi
{
    public int sensorVerisiId { get; set; }

    public double deger { get; set; }

    public string birim { get; set; } = string.Empty;

    public DateTime olcumTarihi { get; set; }

    public bool esikDegerAsildiMi { get; set; }

    // Foreign key
    // Not: Sensor entity'sinde ayrı sensorId olmadığı için bu alan şimdilik kullanılmayabilir.
    public int sensorId { get; set; }

    public virtual Sensor Sensor { get; set; } = null!;

    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;

    public int vardiyaId { get; set; }

    public virtual Vardiya Vardiya { get; set; } = null!;
}
