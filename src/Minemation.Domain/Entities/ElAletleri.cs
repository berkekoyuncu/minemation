using System;
using System.Collections.Generic;
using System.Text;


namespace Minemation.Domain.Entities;

public class ElAletleri
{
    public string gucKaynagiTipi { get; set; } = string.Empty;

    public decimal bataryaKapasitesi { get; set; }

    public string kullanimAmaci { get; set; } = string.Empty;

    // Foreign key
    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;
}
