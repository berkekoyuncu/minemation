using System;
using System.Collections.Generic;
using System.Text;

namespace Minemation.Domain.Entities;

public class Hafriyat
{
    public string plaka { get; set; } = string.Empty;

    public decimal damperHacmi { get; set; }
    public decimal azamiYukAgirligi { get; set; }

    public int dingilSayisi { get; set; }

    // Foreign key
    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;
}
