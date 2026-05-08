using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class VakaRaporu
{
    public string ciddiyetSeviyesi { get; set; }

    public decimal cozumSuresi { get; set; }

    // Foreign key
    public int raporId { get; set; }

    public Rapor Rapor { get; set; }

    // Raporlayan personel
    public int personelId { get; set; }

    public Personel Personel { get; set; }
}
