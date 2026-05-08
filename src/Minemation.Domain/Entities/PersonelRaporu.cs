using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class PersonelRaporu
{
    public string uzmanlikAlani { get; set; }

    public int personelSayisi { get; set; }

    public decimal calismaSuresi { get; set; }

    // Foreign key
    public int raporId { get; set; }

    public Rapor Rapor { get; set; }
}
