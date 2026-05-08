using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Minemation.Domain.Entities;

public class TakipCihazi
{
    public int takipCihaziId { get; set; }

    public string takipCihaziSeriNo { get; set; } = string.Empty;
    public string takipCihaziTuru { get; set; } = string.Empty;
    public string takipCihaziModeli { get; set; } = string.Empty;
    public string takipCihaziDurumu { get; set; } = string.Empty;

    public DateTime takipCihaziSonBaglantiZamani { get; set; }

    public string takipCihaziHaberlesmeProtokolu { get; set; } = string.Empty;

    public decimal pilSeviyesi { get; set; }

    // Foreign key
    public int personelId { get; set; }

    public Personel Personel { get; set; } = null!;

    public int ekipmanId { get; set; }

    public Ekipman Ekipman { get; set; } = null!;
}
