using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class Kirici
{
    public decimal darbeEnerjisi { get; set; }
    public decimal dakikadakiDarbeSayisi { get; set; }
    public decimal calismaBasinci { get; set; }

    public string ucTipi { get; set; } = string.Empty;
    public string gerekenYagDebisi { get; set; } = string.Empty;

    // Foreign key
    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;
}
