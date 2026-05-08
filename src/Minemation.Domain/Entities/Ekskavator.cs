using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class Ekskavator
{
    public string plaka { get; set; } = string.Empty;

    public decimal kovaKapasitesi { get; set; }
    public decimal motorGucu { get; set; }
    public decimal maksimumKaziDerinligi { get; set; }

    public string paletTipi { get; set; } = string.Empty;

    public decimal bomUzunlugu { get; set; }

    // Foreign key
    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;
}